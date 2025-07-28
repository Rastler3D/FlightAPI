namespace Application.Flights.Queries.GetFlights;

public sealed class GetFlightsQueryValidator : AbstractValidator<GetFlightsQuery>
{
    public GetFlightsQueryValidator()
    {
        RuleFor(x => x.Origin)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Origin));

        RuleFor(x => x.Destination)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Destination));
    }
}
