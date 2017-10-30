namespace RealSmartDApps.Utils

module Async = 

    let Sequentially asyncs = 
        let rec run asyncs =
            async {
                if asyncs |> Seq.isEmpty then
                    return []
                else
                    let! s' = asyncs |> Seq.head
                    let! ss' = run (asyncs |> Seq.tail)
                    return s'::ss'
            }
        run asyncs