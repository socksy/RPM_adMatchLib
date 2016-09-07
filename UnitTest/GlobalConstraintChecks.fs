namespace UnitTest

open System
open NUnit.Framework
open RPM.adMatching

(*
  Home for global constraint check unit tests
  Checks like global threshold for maximum views of ads 
  etc. Should be added here.
*)        

[<TestFixture>]
type ``Advertisement Global Constraint Checks``() =        
    [<Test>]
    member this.``Old advertisements should be spared``() = 
        let ads = [{
                    ID="1";
                    StartDate=DateTime.Today.AddDays(-30.0);
                    EndDate=DateTime.Today.AddDays(11.0);
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
                    //This advertisement has more than 5000 (Default max value for advertisement)
                    //So this will be spared from the view for the first time
                    Views = 6000
                }]
        
        Assert.AreEqual( 1, ads |> spareOldAds |> List.length)
        //Changing the maxViews to 7000. So now both advertisements are 
        //ready to be served. 
        RPM.adMatching.maxViews <- 7000
        Assert.AreEqual( 2, ads |> spareOldAds |> List.length)

    [<Test>]
    member this.``Expired advertisements should be dropped``() = 
        let ads = [{
                    ID="1";
                    StartDate=DateTime.Today.AddDays(-30.0);
                    EndDate=DateTime.Today.AddDays(-1.0);
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
                }]
        Assert.AreEqual (1, ads |> dropExpiredAds |> List.length )
