namespace BicycleRental.Domain.Enums;

/// <summary>
/// Represents a type of bicycle that can be rented.
/// </summary>
public enum BicycleType
{
    /// <summary>
    /// City bicycles are designed for comfortable urban riding,
    /// typically with an upright riding position, practical features
    /// like fenders and racks, and a focus on ease of use.
    /// </summary>
    City,

    /// <summary>
    /// Mountain bicycles (MTB) are built for off-road trails,
    /// with sturdy frames, wider knobby tires and often front or full suspension
    /// to handle rough terrain and obstacles.
    /// </summary>
    Mountain,

    /// <summary>
    /// Road bicycles are lightweight and optimized for speed on paved surfaces,
    /// featuring narrow tires, drop handlebars and a more aggressive riding position.
    /// </summary>
    Road,

    /// <summary>
    /// Electric bicycles (e-bikes) include an electric motor and battery
    /// to assist pedaling, making them suitable for longer commutes or hilly routes.
    /// </summary>
    Electric,

    /// <summary>
    /// Sport bicycles are performance-oriented models for cycling enthusiasts,
    /// focusing on high speed, responsive handling and reduced weight.
    /// </summary>
    Sport
}
