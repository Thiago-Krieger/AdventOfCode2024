namespace AdventOfCode2024.Days;

public sealed class Day5
{
    private static readonly string[] ExampleRules;
    private static readonly string[] ExampleUpdates;
    private static readonly string[] InputRules;
    private static readonly string[] InputUpdates;
    
    static Day5()
    {
        var inputExample = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Data/Day5.Example"));
        ExampleRules = inputExample.Where(line => line.Contains('|')).ToArray();
        ExampleUpdates = inputExample.Where(line => line.Contains(',')).ToArray();
        
        var realInput = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Data/Inputs/Day5.Input"));
        InputRules = realInput.Where(line => line.Contains('|')).ToArray();
        InputUpdates = realInput.Where(line => line.Contains(',')).ToArray();
    }


    public static void Problem1(bool runOnExample)
    {
        var rules = runOnExample ? ExampleRules : InputRules;
        var updates = runOnExample ? ExampleUpdates : InputUpdates;
        
        var followingPages = new Dictionary<string, string[]>();
        var forbiddenBackPages = new Dictionary<string, string[]>();
        
        var sum = CheckOrderRecursive(updates, rules, followingPages, forbiddenBackPages, runOnExample, false);
        
        Console.WriteLine($"Result of Day 5 Problem 1: {sum}. Running on example {runOnExample}");
    }
    
    public static void Problem2(bool runOnExample)
    {
        var rules = runOnExample ? ExampleRules : InputRules;
        var updates = runOnExample ? ExampleUpdates : InputUpdates;
        
        var followingPages = new Dictionary<string, string[]>();
        var forbiddenBackPages = new Dictionary<string, string[]>();
        
        var sum = CheckOrderRecursive(updates, rules, followingPages,forbiddenBackPages, runOnExample, true);
        
        Console.WriteLine($"Result of Day 5 Problem 2: {sum}. Running on example {runOnExample}");
    }

    private static int CheckOrderRecursive(string[] updates, string[] rules, Dictionary<string, string[]> followingPages, Dictionary<string, string[]> forbiddenBackPages, bool verbose, bool fixWrongPages = false)
    {
        var sum = 0;
        var fixedPagesSum = 0;
        foreach (var update in updates)
        {
            var isAlreadySorted = false;
            var isReportFixed = false;
            var pages = update.Split(',');

            for (var position = 0; position < pages.Length; position++)
            {
                var page = pages[position];
                
                MapRulesForPage(rules, followingPages, page);
                MapForbiddenPages(rules, forbiddenBackPages, page);

                var wrongBackPages = forbiddenBackPages[page];
                var followingReportCurrentPage = pages.Skip(position + 1).ToArray();
                var brakingRulePage = followingReportCurrentPage.FirstOrDefault(followingPage => wrongBackPages.Contains(followingPage));

                if (brakingRulePage != null)
                {
                    isAlreadySorted = false;

                    if (fixWrongPages)
                    {
                        SwapWrongPositions(pages, page, brakingRulePage);
                        position -= 1;
                        isReportFixed = true;
                        continue;
                    }
                    break;
                }

                var followingRightCurrentPage = followingPages[page];

                if (followingReportCurrentPage.All(followingPage => followingRightCurrentPage.Contains(followingPage)))
                {
                    isAlreadySorted = true;
                    continue;
                }

                isAlreadySorted = false;
            }

            if (!isAlreadySorted)
                continue;

            if (isReportFixed)
            {
                fixedPagesSum = SumMiddleElement(pages, fixedPagesSum);
                if(verbose)
                    Console.WriteLine($"The report: {update} is fixed and became {string.Join(',', pages)}");
            }

            sum = SumMiddleElement(pages, sum);
        }
        return fixWrongPages ? fixedPagesSum : sum;
    }

    private static int SumMiddleElement(string[] pages, int sum)
    {
        var middleElement = pages[(pages.Length - 1)/2];
                
        int.TryParse(middleElement, out var middlePage);
                
        sum += middlePage;

        return sum;
    }

    private static void SwapWrongPositions(string[] pages, string page, string brakingRulePage)
    {
        var destinationIndex = Array.IndexOf(pages, brakingRulePage);
        var currentIndex = Array.IndexOf(pages, page);
        
        pages[destinationIndex] = page;
        pages[currentIndex] = brakingRulePage;
    }

    private static void MapForbiddenPages(string[] rules, Dictionary<string, string[]> forbiddenBackPages, string page)
    {
        if (forbiddenBackPages.ContainsKey(page))
            return;

        var followingCurrentPagePages = rules.Where(rule => rule.EndsWith(page)).Select(rule => rule.Split('|')[0]).ToArray();
        forbiddenBackPages[page] = followingCurrentPagePages;
    }

    private static void MapRulesForPage(string[] rules, Dictionary<string, string[]> followingPages, string page)
    {
        if (followingPages.ContainsKey(page))
            return;
                
        var followingCurrentPagePages = rules.Where(rule => rule.StartsWith(page)).Select(rule => rule.Split('|')[1]).ToArray();
        followingPages[page] = followingCurrentPagePages;
    }
}