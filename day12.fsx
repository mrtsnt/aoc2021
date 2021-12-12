open System.IO

let edges =
    File.ReadAllLines("data/day12")
    |> Array.map (fun r -> r.Split("-").[0], r.Split("-").[1])
    |> Array.collect (fun (s, e) ->
        match s, e with
        | "start", _ -> [|(s, e)|]
        | _, "start" -> [|(e, s)|]
        | "end", _ -> [|(e, s)|]
        | _, "end" -> [|(s, e)|]
        | _, _ -> [|(s, e); (e, s)|])
 
let solve (edges: (string * string)[]) canVisit visit =
    let rec traverse pos visited =
        let nextNodes = Array.filter (fun (s, e) -> pos = s && canVisit e visited) edges
        match Array.isEmpty nextNodes, pos with
        | true, "end" -> 1
        | true,  _ -> 0
        | _ -> Array.sumBy (fun (_, e) -> traverse e (visit e visited)) nextNodes

    traverse "start" Set.empty

let canVisit1 e visited = not (Set.contains e visited)
let visit1 (e: string) visited = if e.ToLower() = e then Set.add e visited else visited

let canVisit2 e (visited: Set<string>) = 
    if Set.isEmpty visited then true
    else
        match Set.contains e visited, Set.minElement visited with
        | true, "_" -> false
        | true, _ -> true
        | _ -> true

let visit2 (e: string) visited = 
    match e.ToLower() = e, Set.contains e visited with
    | true, true -> Set.add "_" visited
    | true, false -> Set.add e visited
    | _ -> visited

solve edges canVisit1 visit1 |> printfn "%d"
solve edges canVisit2 visit2 |> printfn "%d"