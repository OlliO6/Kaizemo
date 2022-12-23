namespace CleanerSignalsGenerator;

using Godot.SourceGenerators;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;

[Generator]
public class CleanerSignalsGenerator : ISourceGenerator
{
    public const string SignalDelegateSuffix = "EventHandler";

    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        AddEmitterInterface(context);

        INamedTypeSymbol[] godotClasses = context
                .Compilation.SyntaxTrees
                .SelectMany(tree =>
                    tree.GetRoot().DescendantNodes()
                        .OfType<ClassDeclarationSyntax>()
                        .SelectGodotScriptClasses(context.Compilation)
                        .Where(x => x.cds.IsPartial())
                        .Select(x => x.symbol)
                )
                .Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default)
                .ToArray();

        if (godotClasses.Length > 0)
        {
            var typeCache = new MarshalUtils.TypeCache(context.Compilation);

            foreach (var godotClass in godotClasses)
            {
                VisitGodotScriptClass(context, typeCache, godotClass);
            }
        }
    }

    private void AddEmitterInterface(GeneratorExecutionContext context)
    {
        context.AddSource("ISignalEmitter.g", SourceText.From(@"
namespace Godot
{
    public interface ISignalEmitter
    {
        Godot.Object Bound { get; set; }
    }
}", Encoding.UTF8));
    }

    private static void VisitGodotScriptClass(
        GeneratorExecutionContext context,
        MarshalUtils.TypeCache typeCache,
        INamedTypeSymbol symbol)
    {
        INamespaceSymbol namespaceSymbol = symbol.ContainingNamespace;
        string classNs = namespaceSymbol != null && !namespaceSymbol.IsGlobalNamespace ?
            namespaceSymbol.FullQualifiedName() :
            string.Empty;
        bool hasNamespace = classNs.Length != 0;

        bool isInnerClass = symbol.ContainingType != null;

        string fullName = symbol.FullQualifiedName();
        string? baseFullName = symbol.BaseType?.FullQualifiedName();

        if (baseFullName == null)
            return;

        string uniqueHint = fullName.SanitizeQualifiedNameForUniqueHint()
                            + "_SignalEmitter.generated";

        var source = new StringBuilder();

        source.Append("using Godot;\n");
        source.Append("using Godot.NativeInterop;\n");
        source.Append("\n");

        if (hasNamespace)
        {
            source.Append("namespace ");
            source.Append(classNs);
            source.Append(" {\n\n");
        }

        if (isInnerClass)
        {
            var containingType = symbol.ContainingType;

            while (containingType != null)
            {
                source.Append("partial ");
                source.Append(containingType.GetDeclarationKeyword());
                source.Append(" ");
                source.Append(containingType.NameWithTypeParameters());
                source.Append("\n{\n");

                containingType = containingType.ContainingType;
            }
        }

        source.Append("partial class ");
        source.Append(symbol.NameWithTypeParameters());
        source.Append("\n{\n");

        var members = symbol.GetMembers();

        var signalDelegateSymbols = members
            .Where(s => s.Kind == SymbolKind.NamedType)
            .Cast<INamedTypeSymbol>()
            .Where(namedTypeSymbol => namedTypeSymbol.TypeKind == TypeKind.Delegate)
            .Where(s => s.GetAttributes()
                .Any(a => a.AttributeClass?.IsGodotSignalAttribute() ?? false));

        bool parentInSameAssembly = symbol.BaseType!.ContainingAssembly.Name == symbol.ContainingAssembly.Name;

        // Generate GetEmitter

        if (!parentInSameAssembly)
        {
            source.Append("    /// <summary>\n")
                .Append("    /// <para>Gets a <see cref=\"ISignalEmitter\"/> to emit a signal.</para>\n")
                .Append("    /// <para>Example:</para>\n")
                .Append("    /// <code>GetEmitter<SignalEmit.SignalName>().Emit();</code>\n")
                .Append("    /// </summary>\n")
                .Append("    /// <returns>A <typeparamref name=\"TSignalEmitter\"/> ready to call <see langword=\"Emit\"/> on.</returns>\n")
                .Append("    public TSignalEmitter GetEmitter<TSignalEmitter>()\n")
                .Append("        where TSignalEmitter : ISignalEmitter, new()\n")
                .Append("    {\n")
                .Append("        return new TSignalEmitter() { Bound = this };\n")
                .Append("    }\n");
        }

        // Generate SignalEmitter

        if (!parentInSameAssembly)
        {
            source.Append("    public class SignalEmitter");
        }
        else
        {
            source.Append("    public new class SignalEmitter : ")
                .Append(baseFullName)
                .Append(".SignalEmitter\n");
        }

        source.Append("\n    {\n");

        foreach (var signalDelegate in signalDelegateSymbols)
        {
            string signalName = signalDelegate.Name.Substring(0, signalDelegate.Name.Length - SignalDelegateSuffix.Length);

            source.Append("        public struct ")
                .Append(signalName)
                .Append(" : ISignalEmitter\n")
                .Append("        {\n");

            source.Append("            public Object Bound { get; set; }\n");

            // Generate Emit method

            var parameters = signalDelegate.DelegateInvokeMethod!.Parameters;
            string @params = "";
            string paramsCall = "";

            for (int i = 0; i < parameters.Length; i++)
            {
                IParameterSymbol parameter = parameters[i];

                if (i > 0)
                    @params += ", ";

                @params += $"{parameter.Type.FullQualifiedName()} {parameter.Name}";

                // Enums must be converted to the underlying type before they can be implicitly converted to Variant
                if (parameter.Type.TypeKind == TypeKind.Enum)
                {
                    var underlyingType = ((INamedTypeSymbol)parameter.Type).EnumUnderlyingType!;
                    paramsCall += $", ({underlyingType.FullQualifiedName()}){parameter.Name}";
                    continue;
                }

                paramsCall += $", {parameter.Name}";
            }

            source.Append("            public void Emit")
                .Append("(")
                .Append(@params)
                .Append(")\n")
                .Append("                => Bound.EmitSignal(")
                .Append(fullName)
                .Append(".SignalName.")
                .Append(signalName)
                .Append(paramsCall)
                .Append(");\n");

            source.Append("        }\n");
        }

        source.Append("    }\n");

        source.Append("}\n"); // partial class

        if (isInnerClass)
        {
            var containingType = symbol.ContainingType;

            while (containingType != null)
            {
                source.Append("}\n"); // outer class

                containingType = containingType.ContainingType;
            }
        }

        if (hasNamespace)
        {
            source.Append("\n}\n");
        }

        context.AddSource(uniqueHint, SourceText.From(source.ToString(), Encoding.UTF8));
    }
}
