namespace Shoko.Server.Filters.Files;

public class HasSubtitleLanguageExpression : FilterExpression<bool>
{
    public HasSubtitleLanguageExpression(string parameter)
    {
        Parameter = parameter;
    }
    public HasSubtitleLanguageExpression() { }

    public string Parameter { get; set; }
    public override bool TimeDependent => false;
    public override bool UserDependent => false;

    public override bool Evaluate(Filterable filterable)
    {
        return filterable.SubtitleLanguages.Contains(Parameter);
    }
}
