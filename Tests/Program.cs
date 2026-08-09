using AutoClientSelector;

var failures = new List<string>();

void Check(string name, bool condition)
{
    if (!condition)
        failures.Add(name);
}

var hashA = new string('A', 64);
var hashB = new string('B', 64);

Check(
    "Newer Workshop mod version is accepted",
    WorkshopUpdateRules.ShouldStageUpdate(
        new Version(1, 0, 0),
        new Version(1, 1, 0),
        100,
        100,
        hashA,
        hashB));
Check(
    "Older Workshop mod version is rejected",
    !WorkshopUpdateRules.ShouldStageUpdate(
        new Version(1, 0, 0),
        new Version(0, 1, 6),
        200,
        100,
        hashA,
        hashB));
Check(
    "Identical Workshop payload is ignored",
    !WorkshopUpdateRules.ShouldStageUpdate(
        new Version(1, 0, 0),
        new Version(1, 0, 0),
        200,
        100,
        hashA,
        hashA));
Check(
    "Newer same-version compatibility build is accepted",
    WorkshopUpdateRules.ShouldStageUpdate(
        new Version(1, 0, 0),
        new Version(1, 0, 0),
        200,
        100,
        hashA,
        hashB));
Check(
    "Previously applied same-version build is ignored",
    !WorkshopUpdateRules.ShouldStageUpdate(
        new Version(1, 0, 0),
        new Version(1, 0, 0),
        100,
        100,
        hashA,
        hashB));
Check(
    "Invalid Workshop payload hash is rejected",
    !WorkshopUpdateRules.ShouldStageUpdate(
        new Version(1, 0, 0),
        new Version(1, 1, 0),
        200,
        100,
        "invalid",
        hashB));

if (failures.Count == 0)
{
    Console.WriteLine("All Auto Client Selector logic tests passed.");
    return 0;
}

foreach (var failure in failures)
    Console.Error.WriteLine($"FAILED: {failure}");
return 1;
