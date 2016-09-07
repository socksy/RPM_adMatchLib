// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System
open RPM.adMatching

[<EntryPoint>]
let main argv = 
    //Sample Channels 
    let Channels = [
                    {
                        ID="1"; 
                        Type = Blog;  
                        URL="abc.com"; 
                        Language="de"; 
                        Country="Germany";
                        UserInterests = ["Fashion";"Automobile"];
                        Constraints = [
                                            (LanguageConstraint,["German"]);
                                            (PreferenceConstraint,["Fashion";"Beauty";"Automobile"])
                                      ]
                     };
                     {
                        ID="2"; 
                        Type = Blog;  
                        URL="abc.com"; 
                        Language="en"; 
                        Country="UK";
                        UserInterests = ["Automobile";"Cars";"Transportation";"Fashion"];
                        Constraints = [
                                            (LanguageConstraint,["English"]);
                                            (PreferenceConstraint,["Fashion";"Cars";"Automobile"])
                                      ]
                     }]
                
    //Sample advertisement
    let ads =  [
                {
                    ID="1";
                    StartDate=DateTime.Today.AddDays(-30.0);
                    EndDate=DateTime.Today.AddDays(30.0);
                    Language="German";
                    Type=["Beauty";"Fashion"];
                    Views = 1
                 };
                {
                    ID="2";
                    StartDate=DateTime.Today.AddDays(-30.0);
                    EndDate=DateTime.Today.AddDays(30.0);
                    Language="Italian";
                    Type=["Beauty";"Fashion"];
                    Views = 2
                };
                {
                    ID="3";
                    StartDate=DateTime.Today.AddDays(-40.0);
                    EndDate=DateTime.Today.AddDays(10.0);
                    Language="German";
                    Type=["Beauty";"Fashion"];
                    Views = 2
                }
              ]

    //First channel is from Germany and preferred language is "German"
    //And that of the 
    printfn "%A" (matchAdv Channels.[0] ads)//should print either 1 or 3
    printfn "%A" (matchAdv Channels.[1] ads)//should print "None"
    0
    
