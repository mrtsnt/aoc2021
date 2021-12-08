open System
open System.IO

let getData () =
    File.ReadAllLines("data/day8")
    |> Array.map (fun r ->
        let split = r.Split(" | ")
        split.[0].Split(" ", StringSplitOptions.RemoveEmptyEntries),
        split.[1].Split(" ", StringSplitOptions.RemoveEmptyEntries))

let part1 (data: (string[] * string[])[]) =
    let getSimpleNumbers arr = 
        arr |> Array.filter (fun (s : string) -> 
            s.Length = 2 || s.Length = 3 || s.Length = 4 || s.Length = 7)

    data |> Array.sumBy (snd >> getSimpleNumbers >> Array.length)

let part2 (data: (string[] * string[])[]) =
    let getNumber (row: string[] * string[]) =
        let getBySize arr sz = Array.find (fun s -> Set.count s = sz) arr
        let allDigits = Array.map set (fst row)

        let i = getBySize allDigits 2
        let l = Set.difference (getBySize allDigits 4) i

        let decode (s: Set<char>) = 
            match Set.count s, Set.isSubset i s, Set.isSubset l s with
            | 7, _, _ -> 8
            | 3, _, _ -> 7
            | 4, _, _ -> 4
            | 2, _, _ -> 1
            | 5, true, false -> 3
            | 5, false, true -> 5
            | 5, false, false -> 2
            | 6, true, true -> 9
            | 6, false, true -> 6
            | 6, true, false -> 0
            | _ -> failwith (sprintf "can't decode %d %A %A" (Set.count s) (Set.isSubset i s) (Set.isSubset l s))

        let map = Array.zip allDigits (Array.map decode allDigits) |> Map.ofArray
        snd row |> Array.map set |> Array.fold (fun s i -> s * 10 + map.[i]) 0

    data |> Array.sumBy getNumber

getData() |> part1 |> printfn "%d"
getData() |> part2 |> printfn "%d"