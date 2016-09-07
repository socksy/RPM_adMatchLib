namespace UnitTest

open System
open System.Linq
open NUnit.Framework
open RPM.adMatching

(*
  Home for channel constraint check unit tests
  Checks like "Germany" should get only "German" speaking ads 
  etc. Should be added here.
*)        

[<TestFixture>]
type ``Advertisement Channel Constraint Checks``() = 
        member this.channels = [
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
        member this.ads =  [
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
                        Views = 1
                    }
                  ]
         [<Test>]
         member this.``Germany should get only German speaking advertisements``() =
            let matchingAdID = matchAdv this.channels.[0] this.ads
            Assert.IsTrue(matchingAdID = "3" || matchingAdID = "1")
           
         [<Test>] 
         member this.``View count should be updated properly``() = 
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
                        Views = 1
                    }
                  ]
            let matchingAdID = matchAdv this.channels.[0] ads
            Assert.AreEqual (2, ads.First( fun t -> t.ID = matchingAdID).Views)
            