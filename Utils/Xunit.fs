namespace RealSmartDApps.Utils

open System
open System.Reflection
open Xunit
open Xunit.Abstractions
open Xunit.Sdk

module XUnit =
    
    [<AllowNullLiteral>]
    type OrderAttribute(i) =
        inherit Attribute()
        member val I = i with get

    type CustomTestCollectionOrderer() =

        let getOrder (testCollection :ITestCollection) = 
            let i = testCollection.DisplayName.LastIndexOf(' ')
            if i <= -1
                then 0
            else
                let className = testCollection.DisplayName.Substring (i + 1)
                let ty = Type.GetType className
                if isNull ty then 0
                else
                    let ti = ty.GetTypeInfo()
                    let attr = ti.GetCustomAttribute<OrderAttribute>()
                    if isNull attr then 0
                    else attr.I

        interface ITestCollectionOrderer with
            member x.OrderTestCollections(testCollections) = testCollections |> Seq.sortBy getOrder

    type CustomTestCaseOrderer() =
        let getOrder (testCase :ITestCase) =
            let mi = testCase.TestMethod.Method.ToRuntimeMethod()
            let attr = mi.GetCustomAttribute<OrderAttribute>()
            if isNull attr then 0
            else
                attr.I

        interface ITestCaseOrderer with
            member x.OrderTestCases(testCases) = testCases |> Seq.sortBy getOrder