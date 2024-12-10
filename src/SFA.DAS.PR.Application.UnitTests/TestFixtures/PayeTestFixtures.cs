namespace SFA.DAS.PR.Application.UnitTests.TestFixtures;

public static class PayeTestFixtures
{
    public static string[] InvalidPayeCases =
    {
        "11/1",
        "11A/1",
        "A11/1",
        "1AA/1",
        "11A/1",
        "A1A1",
        "1",
        "12",
        "123/",
        "11A/1234567",
        "1A1/1234567",
        "A11/12345678",
        "222",
        "222/",
        "222/12345678901"
    };
};
