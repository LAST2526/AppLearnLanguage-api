namespace Last02.Web.Extensions
{
    public static class ListToDictionaryExtension
    {
        public static IDictionary<string, string[]> ToDictionary(this IEnumerable<FluentValidation.Results.ValidationFailure> list)
        {
            return list.GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
        }
    }
}
