using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Timers;

public static class Schedular
{
    private static Timer aTimer;

    public static void Main()
    {
        // Create a timer with a 1 hour interval.
        aTimer = new System.Timers.Timer(3600000); //set interval of 1 day  //3600000 3600000
        // Hook up the Elapsed event for the timer. 
        aTimer.Elapsed += OnTimedEvent;
        aTimer.Enabled = true;

    }


    private static void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        TimeSpan start = new TimeSpan(16, 0, 0); //10 o'clock
        TimeSpan end = new TimeSpan(16, 30, 0); //12 o'clock
        TimeSpan now = DateTime.Now.TimeOfDay;

        if ((now >= start) && (now <= end))
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStringCareGiver"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("deleteremainingpatientrequests", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }



    }
}