using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Godot.SourceGenerators
{
    static class ExtensionMethods
    {
        public static bool TryGetGlobalAnalyzerProperty(
            this GeneratorExecutionContext context, string property, out string? value
        ) => context.AnalyzerConfigOptions.GlobalOptions
            .TryGetValue("build_property." + property, out value);

        public static bool AreGodotSourceGeneratorsDisabled(this GeneratorExecutionContext context)
            => context.TryGetGlobalAnalyzerProperty("GodotSourceGenerators", out string? toggle) &&
               toggle != null &&
               toggle.Equals("disabled", StringComparison.OrdinalIgnoreCase);

        public static bool IsGodotToolsProject(this GeneratorExecutionContext context)
            => context.TryGetGlobalAnalyzerProperty("IsGodotToolsProject", out string? toggle) &&
               toggle != null &&
               toggle.Equals("true", StringComparison.OrdinalIgnoreCase);

        public static bool InheritsFrom(this INamedTypeSymbol? symbol, string assemblyName, string typeFullName)
        {
            while (symbol != null)
            {
                if (symbol.ContainingAssembly?.Name == assemblyName &&
                    symbol.ToString() == typeFullName)
                {
                    return true;
                }

                symbol = symbol.BaseType;
            }

            return false;
        }

        public static INamedTypeSymbol? GetGodotScriptNativeClass(this INamedTypeSymbol classTypeSymbol)
        {
            var symbol = classTypeSymbol;

            while (symbol != null)
            {
                if (symbol.ContainingAssembly?.Name == "GodotSharp")
                    return symbol;

                symbol = symbol.BaseType;
            }

            return null;
        }

        public static string? GetGodotScriptNativeClassName(this INamedTypeSymbol classTypeSymbol)
        {
            var nativeType = classTypeSymbol.GetGodotScriptNativeClass();

            if (nativeType == null)
                return null;

            var godotClassNameAttr = nativeType.GetAttributes()
                .FirstOrDefault(a => a.AttributeClass?.IsGodotClassNameAttribute() ?? false);

            string? godotClassName = null;

            if (godotClassNameAttr is { ConstructorArguments: { Length: > 0 } })
                godotClassName = godotClassNameAttr.ConstructorArguments[0].Value?.ToString();

            return godotClassName ?? nativeType.Name;
        }

        private static bool IsGodotScriptClass(
            this ClassDeclarationSyntax cds, Compilation compilation,
            out INamedTypeSymbol? symbol
        )
        {
            var sm = compilation.GetSemanticModel(cds.SyntaxTree);

            var classTypeSymbol = sm.GetDeclaredSymbol(cds);

            if (classTypeSymbol?.BaseType == null
                || !classTypeSymbol.BaseType.InheritsFrom("GodotSharp", GodotClasses.Object))
            {
                symbol = null;
                return false;
            }

            symbol = classTypeSymbol;
            return true;
        }

        public static IEnumerable<(ClassDeclarationSyntax cds, INamedTypeSymbol symbol)> SelectGodotScriptClasses(
            this IEnumerable<ClassDeclarationSyntax> source,
            Compilation compilation
        )
        {
            foreach (var cds in source)
            {
                if (cds.IsGodotScriptClass(compilation, out var symbol))
                    yield return (cds, symbol!);
            }
        }

        public static bool IsNested(this TypeDeclarationSyntax cds)
            => cds.Parent is TypeDeclarationSyntax;

        public static bool IsPartial(this TypeDeclarationSyntax cds)
            => cds.Modifiers.Any(SyntaxKind.PartialKeyword);

        public static bool AreAllOuterTypesPartial(
            this TypeDeclarationSyntax cds,
            out TypeDeclarationSyntax? typeMissingPartial
        )
        {
            SyntaxNode? outerSyntaxNode = cds.Parent;

            while (outerSyntaxNode is TypeDeclarationSyntax outerTypeDeclSyntax)
            {
                if (!outerTypeDeclSyntax.IsPartial())
                {
                    typeMissingPartial = outerTypeDeclSyntax;
                    return false;
                }

                outerSyntaxNode = outerSyntaxNode.Parent;
            }

            typeMissingPartial = null;
            return true;
        }

        public static string GetDeclarationKeyword(this INamedTypeSymbol namedTypeSymbol)
        {
            string? keyword = namedTypeSymbol.DeclaringSyntaxReferences
                .OfType<TypeDeclarationSyntax>().FirstOrDefault()?
                .Keyword.Text;

            return keyword ?? namedTypeSymbol.TypeKind switch
            {
                TypeKind.Interface => "interface",
                TypeKind.Struct => "struct",
                _ => "class"
            };
        }

        private static SymbolDisplayFormat FullyQualifiedFormatOmitGlobal { get; } =
            SymbolDisplayFormat.FullyQualifiedFormat
                .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted);

        public static string FullQualifiedName(this ITypeSymbol symbol)
            => symbol.ToDisplayString(NullableFlowState.NotNull, FullyQualifiedFormatOmitGlobal);

        public static string NameWithTypeParameters(this INamedTypeSymbol symbol)
        {
            return symbol.IsGenericType ?
                string.Concat(symbol.Name, "<", string.Join(", ", symbol.TypeParameters), ">") :
                symbol.Name;
        }

        public static string FullQualifiedName(this INamespaceSymbol namespaceSymbol)
            => namespaceSymbol.ToDisplayString(FullyQualifiedFormatOmitGlobal);

        public static string SanitizeQualifiedNameForUniqueHint(this string qualifiedName)
            => qualifiedName
                // AddSource() doesn't support angle brackets
                .Replace("<", "(Of ")
                .Replace(">", ")");

        public static bool IsGodotExportAttribute(this INamedTypeSymbol symbol)
            => symbol.ToString() == GodotClasses.ExportAttr;

        public static bool IsGodotSignalAttribute(this INamedTypeSymbol symbol)
            => symbol.ToString() == GodotClasses.SignalAttr;

        public static bool IsGodotMustBeVariantAttribute(this INamedTypeSymbol symbol)
            => symbol.ToString() == GodotClasses.MustBeVariantAttr;

        public static bool IsGodotClassNameAttribute(this INamedTypeSymbol symbol)
            => symbol.ToString() == GodotClasses.GodotClassNameAttr;
    }
}
