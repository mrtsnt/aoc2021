open System.IO
 
let data =
    File.ReadAllLines("data/day12")
    |> Array.map (fun r -> r.Split("-").[0], r.Split("-").[1])
    |> List.ofArray
 
let allEdges (data: (string * string) list) = 
    let startEdges = 
        List.filter (fun (a, b) -> a = "start" || b = "start") data
        |> List.map (fun (a, b) -> if b = "start" then (b, a) else (a, b))
    let endEdges = 
        List.filter (fun (a, b) -> a = "end" || b = "end") data 
        |> List.map (fun (a, b) -> if a = "end" then (b, a) else (a, b))
    let other = 
        List.filter (fun (a, b) -> a <> "start" && a <> "end" && b <> "start" && b <> "end") data
        |> List.collect (fun (a, b) -> [(a, b); (b, a)])
    startEdges @ endEdges @ other
 
let part1 (data: (string * string) list) =
    let edges = allEdges data
 
    let rec findAllPaths (paths: (Set<string> * string list) list) : (Set<string> * string list) list =
        List.collect(fun (visited, path) ->
            let nextNodes = List.filter (fun (s, e) -> List.head path = s && not (Set.contains e visited)) edges
            if List.isEmpty nextNodes then [(visited, path)]
            else
                List.collect (fun (_, (e: string)) ->
                    let newVisited = if e.ToLower() = e then Set.add e visited else visited
                    findAllPaths [(newVisited, e::path)]
                ) nextNodes
        ) paths

    findAllPaths [(Set.empty, ["start"])]
    |> List.map snd
    |> List.filter (fun l -> List.head l = "end")
    |> List.length

let part2 (data: (string * string) list) =
    let edges = allEdges data
 
    let rec findAllPaths (paths: (Set<string> * string list) list) : (Set<string> * string list) list =
        List.collect(fun (visited, path) ->
            let nextNodes = List.filter (fun (s, e) -> List.head path = s && not (Set.contains e visited)) edges
            if List.isEmpty nextNodes then [(visited, path)]
            else
                List.collect (fun (_, (e: string)) ->
                    let newVisited = if e.ToLower() = e then Set.add e visited else visited
                    findAllPaths [(newVisited, e::path)]
                ) nextNodes
        ) paths

    findAllPaths [(Set.empty, ["start"])]
    |> List.map snd
    |> List.filter (fun l -> List.head l = "end")
    |> List.length

part1 data
