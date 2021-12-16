open System
open System.Globalization
open System.IO

type Packet 
    = Value of int * int64 // version, value
    | Operator of int * int * Packet list // version, id, subpackets

let decodePacket (str: string) =
    let charToBinary c = Convert.ToString(Int32.Parse(c, NumberStyles.HexNumber), 2).PadLeft(4, '0')
    Seq.map (string >> charToBinary) str |> Seq.fold (+) ""

let readVersion (str: string) = Convert.ToInt32(str.[0..2], 2)
let readPacketId (str: string) = Convert.ToInt32(str.[3..5], 2)

let readValue (str: string) = 
    let rec readChunks (str: string) = 
        if str.[0] = '0' then [str.[1..4]]
        else str.[1..4]::(readChunks str.[5..])
    let chunks = readChunks str.[6..]
    Convert.ToInt64(List.reduce (+) chunks, 2), str.[List.length chunks * 5 + 6..]

let rec parsePacket (str: string) =
    match readPacketId str with
    | 4 -> 
        let value, rem = readValue str
        Value(readVersion str, value), rem
    | _ -> 
        let subpackets, remaining = readSubPackets str
        Operator(readVersion str, readPacketId str, subpackets), remaining

and readSubPackets (str: string) =
    let lengthId = Int32.Parse(string str.[6])
    if lengthId = 0 then
        let totalLength = Convert.ToInt32(str.[7..21], 2)
        let mutable lengthSoFar, packets, remTotal = 0, [], str.[22..]
        while lengthSoFar <> totalLength do
            let subPackets, rem = parsePacket remTotal
            remTotal <- rem
            packets <- packets @ [subPackets]
            lengthSoFar <- str.[22..].Length - rem.Length
        packets, remTotal
    else
        let totalPackets = Convert.ToInt32(str.[7..17], 2)
        List.fold (fun (packets, str) _ -> 
            let packet, remaining = parsePacket str
            packets @ [packet], remaining
        ) ([], str.[18..]) [1..totalPackets]

let data = File.ReadAllText("data/day16")
let parsed = decodePacket data |> parsePacket |> fst

let rec part1 = function
    | Value(v, _) -> v
    | Operator(v, _, ps) -> v + List.sumBy part1 ps

let part2 packet = 
    let applyOp op = function
        | [x;x'] -> if op x x' then 1L else 0L
        | _ -> failwith "expected only two subpacks"

    let rec calc packet =
        match packet with 
        | Value(_, v) -> v
        | Operator(_, op, subPackets) ->
            let calced = List.map calc subPackets
            match op with
            | 0 -> List.sum calced
            | 1 -> List.reduce (*) calced
            | 2 -> List.min calced
            | 3 -> List.max calced
            | 5 -> applyOp (>) calced
            | 6 -> applyOp (<) calced
            | 7 -> applyOp (=) calced
            | _ -> failwith $"unknow op: {op}"
    
    calc packet

parsed |> part1 |> printfn "%d"
parsed |> part2 |> printfn "%d"