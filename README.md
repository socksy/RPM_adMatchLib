# RPM_adMatchLib
Advertisement matching 

### Channels
Channels are represented by the following type 

```fsharp 

type Channel = {
                    ID : string;
                    Type: ChannelType; 
                    URL : string ; 
                    Language : string; 
                    Country : string ; 
                    UserInterests : string list;
                    Constraints : Constraint list 
               }
```

#### Channel Constraints
A constraint is represetented as a tuple of a string and a string list as shown below . And a channel can have multiple such constraints. 

```fsharp
type Constraint = (string * string list)
```
### Advertisement
Advertisements are represetend by the following type 

```fsharp
type Advertisement = {
                        ID : string;
                        StartDate : DateTime; 
                        EndDate : DateTime; 
                        Language : string; //Language of the advertisement
                        Type : string list;//What is the ad about 
                        mutable Views : int 
                     }
```

### Matching function
The function that matches a given channel with the available advertisement has the following signature. 
This function is located at ```/Library1/matcher.fs```

```fsharp
  let matchAdv (request:Channel) (ads: Advertisement list) =
```

The function generates F# functions from the constraints and runs this generated list of functions on advertisement list. Finally uses set intersection to find those advertiements that match all the constraints. The function matchAdv returns a randomly selected advertisement should there be more number of matches. Otherwise if no matching advertisement is found, it returns "None". 

A few sample channel and sample advertisement can be represented as follows. These sample data are used in the program
for executing couple of sample cases. 

### Sample Data (used in the program)

```fsharp
    let Channels = [
                    {
                        ID="1"; 
                        Type = Blog;  
                        URL="abc.com"; 
                        Language="de"; 
                        Country="Germany";
                        UserInterests = ["Fashion";"Automobile"];
                        Constraints = [
                                            ("Language",["German"]);
                                            ("PreferenceOrder",["Fashion";"Beauty";"Automobile"])
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
                                            ("Language",["English"]);
                                            ("PreferenceOrder",["Fashion";"Cars";"Automobile"])
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
```

### Sample Calls (to ```MatchAdv``` function )
As you see from the data, that advertisement "1" and "3" are matching as per the constraints described in the first channel, ```channels.[0]``` 

So the output of 

```fsharp
 printfn "%A" (matchAdv Channels.[0] ads)
 ```
should be either "1" or "3". 

And the output of 

```fsharp
printfn "%A" (matchAdv Channels.[1] ads)
```

should be "None" as there is no advertisement as per the constraint defined for the second channel. 


