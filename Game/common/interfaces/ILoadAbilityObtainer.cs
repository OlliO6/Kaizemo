namespace Game;

using System;

public interface ILoadAbilityObtainer
{
    event Action LoadedAbilityChanged;

    void ObtainLoadAbility(LoadAbility loadAbility);

    LoadAbility? GetLoadedAbility();
}
