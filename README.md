# Feliz.2Sxc [![Nuget](https://img.shields.io/nuget/v/Feliz.2sxc?style=flat-square)](https://www.nuget.org/packages/Feliz.2sxc/)
Bindings and Helpers to facilitate creating apps for [2sxc](https://2sxc.org) with [Feliz](zaid-ajaj.github.io/feliz/) and [Fable](https://fable.io/).

## Features

### 2sxc Context
Typed interface to the main 2sxc internal api.

### API
Uses [Fable.SimpleHttp](https://github.com/Zaid-Ajaj/Fable.SimpleHttp) and [Thoth.Json](https://github.com/thoth-org/Thoth.Json) for a fresh async interface to setup requests against 2sxc data repositories and visual queries.

### Toolbar Builder

Setting up a 2sxc toolbar was never easier, the toolbar builder provides a typed interface including documentation.

Then inject it with `prop.toolbar` and starts it with new hook `React.useToolbarRef`. When a 2sxc dialog closes, it calls the `referesh` callback.

```fsharp
let toolbar (person: PersonInfo option) =
    match person with
    | Some p ->
        Sxc.toolbar [ bi.toolbar.empty [ cp.contentType "Person"
                                         cp.entityId p.Id ]
                      bi.command.edit ()
                      bi.command.delete (
                          [ bp.color color.gray ], 
                          [ cp.entityGuid p.Guid
                            cp.title p.Name ])
                      bi.command.custom (
                          [ bp.color color.seaGreen
                            bp.title "Open 2sxc"
                            bp.icon "icon-sxc-glasses" ],
                          [ cp.openInNewTab "https://2sxc.org/" ]
                      )
                      bi.settings [ se.hover.right ] ]
    | _ ->
        Sxc.toolbar [ bi.toolbar.empty [ cp.contentType "Person" ]
                      bi.command.new' () ]

[<ReactComponent>]
let Person (sxc: Sxc.Context) refresh person =
    Html.li [ prop.toolbar (toolbar (Some person))
              prop.ref (React.useToolbarRef sxc refresh)
              prop.key person.Id
              prop.children [ [ Html.img [ prop.src $"{person.Photo}"
                                           prop.className "person" ]
                                Html.strong person.Name ]
                              |> Html.div ] ]
```

### Routing just works

This part is now a separate package: [Feliz.Router.BasePath](https://github.com/DnnFable/Feliz.Router.BasePath).

```
Install-Package Feliz.Router.BasePath
```