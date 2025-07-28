namespace Application.Flights.Commands.CreateFlight;

public sealed class CreateFlightCommandValidator : AbstractValidator<CreateFlightCommand>
{
    public CreateFlightCommandValidator()
    {
        RuleFor(x => x.Origin)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Destination)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Departure)
            .NotEmpty()
            .GreaterThan(DateTimeOffset.UtcNow);

        RuleFor(x => x.Arrival)
            .NotEmpty()
            .GreaterThan(x => x.Departure);

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
