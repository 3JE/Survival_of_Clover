
/* Summary
 * Clover is a class which stores the information of clover & sync with Azure
 */
public class Clover
{
    public string ID { get; set; } // playerID
    public int c_level = 1; // level of clover
    public string c_name = string.Empty; // name of clover
    public float c_sun_stat = 50; // sun stat of clover
    public float c_water_stat = 50; // water stat of clover
    public float c_energy_stat = 20; // energy stat of clover
    public float c_energy_requirement = 100; // clover's energy requirement to levelup
    public int c_weather_ID = 0; // current weather : 0 = sunny / 1 = cloudy / 2 = rainy
    public int c_bug_ID = -1;   // bug near clover : -1 = nothing / 0 = ladybug / 2 = bee / 3 = aphid
    public int c_bug_time = -1; // the remaining time of bug accurance(unit : sec)
    public int weatherCycle = 60; // the cycle of changing weather(unit : sec)
}
