using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Atmos.Rotting;

/// <summary>
/// This makes mobs eventually start rotting when they die.
/// It may be expanded to food at some point, but it's just for mobs right now.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause]
[Access(typeof(SharedRottingSystem))]
public sealed partial class PerishableComponent : Component
{
    /// <summary>
    /// How long it takes after death to start rotting.
    /// </summary>
    [DataField]
    public TimeSpan RotAfter = TimeSpan.FromMinutes(10);

    /// <summary>
    /// How much rotting has occured
    /// </summary>
    [DataField]
    public TimeSpan RotAccumulator = TimeSpan.Zero;

    /// <summary>
    /// Gasses are released, this is when the next gas release update will be.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan RotNextUpdate = TimeSpan.Zero;

    /// <summary>
    /// How often the rotting ticks.
    /// Feel free to tweak this if there are perf concerns.
    /// </summary>
    [DataField]
    public TimeSpan PerishUpdateRate = TimeSpan.FromSeconds(5);

    /// <summary>
    /// How many moles of gas released per second, per unit of mass.
    /// RMC14 default to 0 to prevent any organic species (including base SS14) from generating gas
    /// </summary>
    [DataField]
    public float MolsPerSecondPerUnitMass = 0;

    [DataField, AutoNetworkedField]
    public int Stage;

    /// <summary>
    /// If true, rot will always progress.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool ForceRotProgression;

    /// <summary>
    /// If true, it ignores human body temperature.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool IgnoreTemperature;
}


[ByRefEvent]
public record struct IsRottingEvent(bool Handled = false);
