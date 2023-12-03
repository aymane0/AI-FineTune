// See https://aka.ms/new-console-template for more information

using gpt_remnant;

string[] items = new[]
{
    /* Increases Ammo Reserves by 25%.
     * https://remnant2.wiki.fextralife.com/file/Remnant-2/deep_pocket_ring_rings_remnant2_wiki_guide_250px.png
     * https://remnant2.wiki.fextralife.com/Deep+Pocket+Ring
     */
    "Deep Pocket Ring",

    /* Increases Melee Charge Speed by 20% and reduces Melee Stamina Cost by 25%.
     * https://remnant2.wiki.fextralife.com/file/Remnant-2/berserkers_crest_rings_remnant2_wiki_guide_250px.png
     * https://remnant2.wiki.fextralife.com/Berserker's+Crest
     */
    "Berserker's Crest"
    ,
    /* Charged Melee Attacks apply BLEEDING, dealing 460 BLEED damage over 20s.
     * https://remnant2.wiki.fextralife.com/file/Remnant-2/blood_jewel_rings_remnant2_wiki_guide_250px.png
     * https://remnant2.wiki.fextralife.com/Blood+Jewel
     */
    "Blood Jewel",

    /* Increases all Elemental damage dealt by 10%.
     * https://remnant2.wiki.fextralife.com/file/Remnant-2/alumini_ring_rings_remnant2_wiki_guide_250px.png
     * https://remnant2.wiki.fextralife.com/Alumni+Ring
     */
    "Alumni Ring",
};

string apiKey = OpenAIConfig.GetApiKey();
OpenAIClient api = new(apiKey);
string systemPrompt;
string userPrompt;
string model;

//systemPrompt = "You are a helpful assistant for a Discord server about the game Remnant 2. Give a short 1 line answer on what the requested item does in this format: The `[item name]` ... .";
//foreach (var item in items)
//{
//    userPrompt = $"What does the ring `{item}` do?";
//    for (int i = 0; i < 4; i++)
//    {
//        var response = await api.GetAnswerForItem(systemPrompt, userPrompt);
//        Console.WriteLine($"`{item}` => {response}");
//    }
//}

//systemPrompt = "You are a helpful assistant for a Discord server about the game Remnant 2.\n";
//systemPrompt += "Give a short 1 line answer on what the requested item does in this format: The `[item name]` ... .\n";
//systemPrompt += "Data:\n";
//systemPrompt += "Deep Pocket Ring: Increases Ammo Reserves by 25%.\n";
//systemPrompt += "Berserker's Crest: Increases Melee Charge Speed by 20% and reduces Melee Stamina Cost by 25%.\n";
//systemPrompt += "Blood Jewel: Charged Melee Attacks apply BLEEDING, dealing 460 BLEED damage over 20s.\n";
//systemPrompt += "Alumni Ring: Increases all Elemental damage dealt by 10%.\n";
//systemPrompt = systemPrompt.Trim();
//Console.WriteLine(systemPrompt);
//foreach (var item in items)
//{
//    userPrompt = $"What does the ring `{item}` do?";
//    for (int i = 0; i < 4; i++)
//    {
//        var response = await api.GetAnswerForItem(systemPrompt, userPrompt);
//        Console.WriteLine($"`{item}` => {response}");
//    }
//}

systemPrompt = "You are a helpful assistant for a Discord server about the game Remnant 2. Give a short 1 line answer on what the requested item does in this format: The `[item name]` ... .";
systemPrompt = "";
model = "ft:gpt-3.5-turbo-1106:personal::8RmdLDnw";
Console.WriteLine($"Using model {model}");
foreach (var item in items)
{
    userPrompt = $"What does the ring `{item}` do?";
    for (int i = 0; i < 4; i++)
    {
        var response = await api.GetAnswerForItem(systemPrompt, userPrompt, model: model);
        Console.WriteLine($"`{item}` => {response}");
    }
}
