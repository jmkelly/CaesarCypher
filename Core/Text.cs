using Markov;

public class Text
{
    public static string GetText()
    {
        return @"As Sarah navigated through the emails, the office thermostat showed a comfortable 72°F, a rare win in the eternal battle between coworkers over temperature preferences. She took a brief break to check her fitness tracker – 3,000 steps achieved, a small victory considering the sedentary nature of her work. 

Meanwhile, Jake's bakery was a hive of activity. The cash register chimed with each transaction, totaling $200 in sales by mid-morning. The scent of brewing coffee blended with the buttery richness of pastries, creating a sensory haven. The bell above the door jingled as a regular customer, a friendly retiree named Mr. Johnson, entered. His order, a cup of black coffee ☕ and a blueberry muffin, was a daily ritual.

In the park, a musician strummed a guitar, earning a few dollars from passersby. The temperature rose to a pleasant 78°F, and the ice cream vendor had a brisk business with cones priced at $3 each. Couples strolled hand in hand, creating a picturesque scene against the backdrop of a fountain. The world continued its cadence of numbers, emotions, and simple joys.";
    }

    public static string GetRandomishButRealisticText(int length)
    {
        var chain = new MarkovChain<string>(1);
        var random = new Random(length);

        //read our sample text
        var text = Text.GetText();

        chain.Add(text.Split(' '));

        string data = string.Empty;
        bool toShort = true;
        while (toShort)
        {
            data = string.Join(" ", chain.Chain(random.Next()));
            if (data.Length >= length)
                toShort = false;
        }


        return data.Substring(0, length);
    }
}

