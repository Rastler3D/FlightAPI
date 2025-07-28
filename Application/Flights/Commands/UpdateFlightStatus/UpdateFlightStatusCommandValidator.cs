namespace Application.Flights.Commands.UpdateFlightStatus;

public sealed class UpdateFlightStatusCommandValidator : AbstractValidator<UpdateFlightStatusCommand>
{
    public UpdateFlightStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
