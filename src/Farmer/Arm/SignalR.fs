[<AutoOpen>]
module Farmer.Arm.SignalRService

open Farmer
open Farmer.CoreTypes
open Farmer.SignalR

type SignalR =
    { Name : ResourceName
      Location : Location
      Sku : Sku
      Capacity : int option
      AllowedOrigins : string list }
    interface IArmResource with
        member this.ResourceName = this.Name
        member this.JsonModel =
            {| ``type`` = "Microsoft.SignalRService/signalR"
               apiVersion = "2018-10-01"
               name = this.Name.Value
               location = this.Location.ArmValue
               sku =
                   {| name =
                         match this.Sku with
                         | Free -> "Free_F1"
                         | Standard -> "Standard_S1"
                      capacity =
                          match this.Capacity with
                          | Some c -> c.ToString()
                          | None -> null |}
               properties =
                   {| cors =
                          match this.AllowedOrigins with
                          | [] -> null
                          | aos -> box {| allowedOrigins = aos |} |}
            |} :> _