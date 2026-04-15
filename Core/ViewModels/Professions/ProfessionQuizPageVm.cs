namespace Core.ViewModels.Professions
{
    public class ProfessionQuizPageVm
    {
        public IReadOnlyList<ProfessionQuizQuestionVm> Questions { get; set; } = Array.Empty<ProfessionQuizQuestionVm>();

        public IReadOnlyList<ProfessionQuizResultVm> Results { get; set; } = Array.Empty<ProfessionQuizResultVm>();
    }

    public class ProfessionQuizQuestionVm
    {
        public int Id { get; set; }

        public string Section { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

        public string? HelperText { get; set; }

        public int MaxSelections { get; set; } = 1;

        public IReadOnlyList<ProfessionQuizOptionVm> Options { get; set; } = Array.Empty<ProfessionQuizOptionVm>();
    }

    public class ProfessionQuizOptionVm
    {
        public string Text { get; set; } = string.Empty;

        public IReadOnlyDictionary<string, int> Scores { get; set; } = new Dictionary<string, int>();
    }

    public class ProfessionQuizResultVm
    {
        public string ProfessionName { get; set; } = string.Empty;

        public int? ProfessionId { get; set; }

        public string Summary { get; set; } = string.Empty;
    }
}
