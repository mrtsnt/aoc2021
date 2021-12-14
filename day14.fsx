open System.IO

let data = File.ReadAllText("data/day14")
let polymer = data.Split("\n\n").[0] |> Seq.toList
let rules = 
    data.Split("\n\n").[1].Split("\n") 
    |> Array.map (fun (r: string) -> 
        let split = r.Split(" -> ")
        List.ofSeq split.[0], split.[1].[0])
    |> Map.ofArray

let part1 polymer (rules: Map<char list, char>)=
    let rec step polymer =
        match polymer with
        | x::x'::xs -> x::(rules.[[x;x']])::(step (x'::xs))
        | _ -> polymer

    let counts = 
        List.fold (fun oldPolymer _ -> step oldPolymer) polymer [1..10]
        |> List.groupBy id
        |> List.map (fun (_, v) -> List.length v)

    List.max counts - List.min counts

let part2 polymer (rules: Map<char list, char>) = 
    let polymerPairs = 
        List.pairwise polymer
        |> List.groupBy id
        |> List.map (fun ((fc, sc), v) -> [fc; sc], List.length v |> int64)
        |> Map.ofList

    let charCounts = 
        List.groupBy id polymer 
        |> List.map (fun (k, v) -> k, List.length v |> int64)
        |> Map.ofList

    let step polymer charCounts =
        Map.fold (fun (polymer, counts) k v ->
            let insertChar = rules.[k]
            let newPolymer = 
                List.fold (fun p pair -> 
                    Map.change pair (function Some n -> Some (n + v) | None -> Some v) p
                ) polymer [[k.[0]; insertChar]; [insertChar; k.[1]]]
            let newCounts = Map.change insertChar (function Some n -> Some (n + v) | None -> Some v) counts
            newPolymer, newCounts
        ) (Map.empty, charCounts) polymer

    let counts = 
        List.fold (fun (pp, cc) _ -> step pp cc) (polymerPairs, charCounts) [1..40]
        |> snd
        |> Map.toList
        |> List.map snd

    List.max counts - List.min counts

part1 polymer rules |> printfn "%d"
part2 polymer rules |> printfn "%d"