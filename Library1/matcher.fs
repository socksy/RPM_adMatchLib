

module RPM.adMatching

open System
open System.Linq

type ChannelType = Blog 
                 | Site 
type ConstraintType = LanguageConstraint | PreferenceConstraint | MaxViewConstraint
type Constraint = (ConstraintType * string list)  

let mutable maxViews = 5000

type Channel = {
                    ID : string;
                    Type: ChannelType; 
                    URL : string ; 
                    Language : string; 
                    Country : string ; 
                    UserInterests : string list;
                    Constraints : Constraint list 
               }

type Advertisement = {
                        ID : string;
                        StartDate : DateTime; 
                        EndDate : DateTime; 
                        Language : string; //Language
                        Type : string list;//What is the ad about 
                        mutable Views : int 
                     }



let dropExpiredAds (ads:Advertisement list) =
     ads |> List.filter (fun ad -> DateTime.Today.CompareTo(ad.EndDate) < 0)

//This is a global constraint
let spareOldAds(ads:Advertisement list)=
    ads |> List.filter (fun ad -> ad.Views < maxViews)


let checkForLanguage (ad:Advertisement)(values:string list)=
    values.Contains ad.Language

let checkForPreferenceOrder(ad:Advertisement)(values:string list)=
   //This is now a vanilla matching of string containing 
   //This can be improved by some other sophisticated matching like
   //Jaccard Index
    ad.Type |> List.filter(fun t -> values.Contains t)|>List.length > 0
    
let getConstraintChecker (tag:ConstraintType)=
    match tag with 
     | ConstraintType.LanguageConstraint -> checkForLanguage
     | ConstraintType.PreferenceConstraint -> checkForPreferenceOrder
     
    
let getCommon (ids : (string list) list) =
    ids |> Seq.map (fun fs -> fs |> Set.ofList) 
        |> Set.intersectMany 
 
let incrementView (ad:Advertisement) =
    ad.Views <- ad.Views + 1

//Matches ads with the request channel 
let matchAdv (request:Channel) (ads: Advertisement list) =
    //Get rid of globally expired ads by "EndDate"
    //And ads that have been viewed more than a set threshold
    let liveAds = ads |> dropExpiredAds 
                      |> spareOldAds  
    //Find checkers (predicates) to be applied on advertisements
    let constraintCheckers = request.Constraints 
                                   |> List.map(fun constraints -> getConstraintChecker (fst constraints))
    //Find values for constraints. 
    let constraintValues = request.Constraints |> List.map snd 
    //Combining these two 
    let constraintValuesAndCheckers = List.zip constraintValues constraintCheckers 
    //This contains the IDs of all the ads for which at least one of the predicate
    //(Constraint function) returns true
    let filtered = constraintValuesAndCheckers 
                    |> List.map(fun valueAndChecker -> 
                                  liveAds 
                                       |> List.filter (fun ad -> (snd valueAndChecker) ad (fst valueAndChecker)))
                    |> List.map (fun filteredAds -> filteredAds |> List.map (fun ad -> ad.ID))   
    //Finds only the intersection of all such ids 
    //resulting in a seq of ids of only the matching advertisements
    let ids = filtered |> getCommon 

    //Pick a random ad in case there are many.
    //if not, it will pick the only matching one.
    let adID = if (ids|>Seq.length) >0 then (ids |> Seq.toList).[(new Random()).Next(ids.Count)] 
                            else "None"
    if adID <> "None" then incrementView (ads.First(fun t -> t.ID = adID))
    adID
    
