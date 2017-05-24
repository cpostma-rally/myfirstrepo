using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Rally.RestApi;
using Rally.RestApi.Response;
using Rally.RestApi.Json;

namespace FindStoriesAndTheirTasksByInCurrentSprint
{
    class Program
    {
        static void Main(string[] args)
        {
            int storyCount = 0;
            int taskCount = 0;
            // RallyRestApi restApi = new RallyRestApi(webServiceVersion: "v2.0");
            // restApi.Authenticate("user@rallydev.com", "PassWOrd", "https://rally1.rallydev.com", allowSSO: false);
            RallyRestApi restApi = new RallyRestApi();
            restApi.AuthenticateWithApiKey("_1nwYJXnBRsi1EbI8kSHi1cDfCC39jUu5lNXUhrGH6YY", new Uri("https://rally1.rallydev.com"));

            String workspaceRef = "/workspace/19487099344";     //replace this OID with an OID of your workspace

            Request sRequest = new Request("HierarchicalRequirement");
            sRequest.Workspace = workspaceRef;
            sRequest.Fetch = new List<string>() { "FormattedID", "Name", "Tasks", "Project", "Release", "Milestones" };
            sRequest.Query = new Query("(Project.Name = CP2015)");
            QueryResult queryResults = restApi.Query(sRequest);

            foreach (var s in queryResults.Results)
            {
                //dynamic rname = new DynamicJsonObject();
                //rname = (s["Release"]);
                //Console.WriteLine("FormattedID: " + s["FormattedID"] + " Name: " + s["Name"] + " Release:  " + rname.Name);
                Console.WriteLine("FormattedID: " + s["FormattedID"] + " Name: " + s["Name"] + " Release:  " + s["Release"]._refObjectName);
                Request milestoneRequest = new Request(s["Milestones"]);
                QueryResult milestoneResult = restApi.Query(milestoneRequest);
                if (milestoneResult.TotalResultCount > 0)
                {
                    foreach (var m in milestoneResult.Results)
                    {
                        Console.WriteLine("Milestone:  " + m["FormattedID"] + "   " + m["Name"]);
                    }

                }
                storyCount++;
                Request tasksRequest = new Request(s["Tasks"]);
                QueryResult queryTaskResult = restApi.Query(tasksRequest);
                if (queryTaskResult.TotalResultCount > 0)
                {
                    foreach (var t in queryTaskResult.Results)
                    {
                        Console.WriteLine("Task: " + t["FormattedID"] + " State: " + t["State"]);
                        taskCount++;
                    }
                }
                Console.WriteLine(storyCount + " stories, " + taskCount + " tasks ");
            }
        }
    }
}