//api 1 dm8dr4oMCGRQa00FfLj_l3CxmnJ3Tw_6
//api 2 prLwyGH3gxa6O11A9C26lR7SRL0nDPhh

using api_KIT;

var client = new ClientKIT("dm8dr4oMCGRQa00FfLj_l3CxmnJ3Tw_6");

Console.WriteLine(await client.CalculatePrice("Ижевск", 12, 0.07));


