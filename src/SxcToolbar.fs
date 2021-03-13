module SxcToolbar

open Fable.Core
open System

/// https://docs.2sxc.org/basics/browser/edit-ux/toolbars/customize.html

type ISettingParameter = interface end

type ICallParameter = interface end

type IBuildParameter = interface end

type IBuildInstruction = interface end

[<RequireQualifiedAccess>]
module Interop =

    let inline mkCallParam (key: string) (value: obj) : ICallParameter = unbox (key + "=" + (unbox value))
    let inline mkBuildParam (key: string) (value: obj) : IBuildParameter = unbox (key + "=" + (unbox value))
    let inline mkSettingParam (key: string) (value: obj) : ISettingParameter = unbox (key + "=" + (unbox value))

    let inline mkCallParams (callParams: ICallParameter seq) =
        match callParams with
        | ps when Seq.isEmpty ps -> ""
        | ps -> "?" + System.String.Join("&", ps)

    let inline mkBuildParams (buildParams: IBuildParameter seq) =
        match buildParams with
        | ps when Seq.isEmpty ps -> ""
        | ps -> "&" + System.String.Join("&", ps)
    
    let inline mkSettingParams (settingParams: ISettingParameter seq) =
        match settingParams with
        | ps when Seq.isEmpty ps -> ""
        | ps -> "&" + System.String.Join("&", ps)

module group  =
    let [<Literal>] default' = "default"
    let [<Literal>] list = "list"
    let [<Literal>] editAdvanced = "edit-advanced"
    let [<Literal>] view  = "view "
    let [<Literal>] app = "app"

module color =
    let [<Literal>] aqua = "aqua"
    let [<Literal>] black = "black"
    let [<Literal>] blue = "blue"
    let [<Literal>] fuchsia = "fuchsia"
    let [<Literal>] gray = "gray"
    let [<Literal>] green = "green"
    let [<Literal>] lime = "lime"
    let [<Literal>] maroon = "maroon"
    let [<Literal>] navy = "navy"
    let [<Literal>] olive = "olive"
    let [<Literal>] orange = "orange"
    let [<Literal>] purple = "purple"
    let [<Literal>] red = "red"
    let [<Literal>] silver = "silver"
    let [<Literal>] teal = "teal"
    let [<Literal>] white = "white"
    let [<Literal>] yellow = "yellow"

/// Build-Parameter
[<Erase>]
type bp =
    /// Gives the button another color
    static member inline color(color: string) : IBuildParameter = Interop.mkBuildParam "color" (color.Trim('#'))
    /// Allows you to set an alternate icon
    static member inline icon(iconClassName: string) : IBuildParameter = Interop.mkBuildParam "icon" iconClassName
    /// Force show/hide a button
    static member inline show(show: bool) : IBuildParameter = Interop.mkBuildParam "icon" show
    /// Add one or more classes to the button to affect styling
    static member inline className(className: string) : IBuildParameter = Interop.mkBuildParam "class" className
    /// Mouseover message
    static member inline title(title: string) : IBuildParameter = Interop.mkBuildParam "title" title 

/// Call-Parameter
[<Erase>]
type cp =
    /// Mainly used for adding new items
    static member inline contentType text : ICallParameter = Interop.mkCallParam "contentType" text
    /// Mainly used for edit, delete etc
    static member inline entityId(id: int) : ICallParameter = Interop.mkCallParam "entityId" id
    // Mainly used for delete
    static member inline entityGuid guid : ICallParameter = Interop.mkCallParam "entityGuid" guid
    /// Mainly used to show a title when asking to delete something
    static member inline title(title: string)  : ICallParameter = Interop.mkCallParam "title" title
    static member inline isPublished(isPublished:bool)  : ICallParameter = Interop.mkCallParam "isPublished" isPublished
    /// For list management, please add also *sortOrder*
    static member inline useModuleList : ICallParameter = Interop.mkCallParam "useModuleList" "true"
    /// for list management, please add also *useModeList*
    static member inline sortOrder(id: int) : ICallParameter = Interop.mkCallParam "sortOrder" id
    /// Custom Code to be called. 
    static member inline customCode (code: string): ICallParameter = Interop.mkCallParam "customCode" code
    /// Navigates to *url*. Works with custom commands, don't combine with customCode
    static member inline navigate (url: string): ICallParameter = Interop.mkCallParam "customCode" ("window.open('" + url + "');")
    /// Opens *url* in new tab or window. Works with custom commands, don't combine with customCode
    static member inline openInNewTab (url: string): ICallParameter = Interop.mkCallParam "customCode" ("window.open('" + url + "','_blank');")
 
/// Setting Parameter
[<Erase>]
module se =
    /// sets default color for all buttons 
    let inline color(color: string) : ISettingParameter = Interop.mkSettingParam "color" color
    /// sets classes for all buttons 
    let inline classes(classes: string) : ISettingParameter = Interop.mkSettingParam "classes" color

    [<Erase>]
    type align =
        
        static member inline left : ISettingParameter = Interop.mkSettingParam "align" "left"
        static member inline right : ISettingParameter = Interop.mkSettingParam "align" "right"

    [<Erase>]
    type hover =
        /// Toolbar opens on hover on the left
        static member inline left : ISettingParameter = Interop.mkSettingParam "hover" "left"
        /// Toolbar opens on hover on the right
        static member inline right : ISettingParameter = Interop.mkSettingParam "hover" "right"
        /// Toolbar don't open on hover
        static member inline none : ISettingParameter = Interop.mkSettingParam "hover" "none"

    [<Erase>]
    type autoAddMore =
        static member inline left : ISettingParameter = Interop.mkSettingParam "autoAddMore" "left"
        static member inline right : ISettingParameter = Interop.mkSettingParam "autoAddMore" "right"

    [<Erase>]
    type follow =
        static member inline none : ISettingParameter = Interop.mkSettingParam "follow" "none"
        static member inline initial : ISettingParameter = Interop.mkSettingParam "follow" "initial"
        static member inline scroll : ISettingParameter = Interop.mkSettingParam "follow" "scroll"
        static member inline always : ISettingParameter = Interop.mkSettingParam "follow" "always"

    [<Erase>]
    type show =
        static member inline hover : ISettingParameter = Interop.mkSettingParam "show" "hover"
        static member inline always : ISettingParameter = Interop.mkSettingParam "show" "always"


/// Build-Instructions
[<Erase>]
module bi =

    let inline group groupname :IBuildInstruction = unbox("group=" + groupname )   
    let inline settings (se: ISettingParameter list) = unbox ("settings" + Interop.mkSettingParams se)   

    [<Erase>]
    type toolbar =
        static member inline empty(callParams: ICallParameter seq) : IBuildInstruction = unbox ("toolbar=empty" + Interop.mkCallParams callParams)

        static member inline default'(callParams: ICallParameter seq) : IBuildInstruction =
            unbox ("toolbar=default?" + Interop.mkCallParams callParams)

    [<Erase>]
    type command =
        /// Open the edit-dialog for a new content-item.  
        /// * contentType
        /// 
        /// Then it needs either the ID...:
        /// * entityId
        /// 
        /// ...or it needs the position within the list:
        /// * useModuleList: true
        /// * sortOrder: [number] (important so it knows the position
        static member inline new'((identifier:string), (buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( identifier + "=new" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Open the edit-dialog for a new content-item.  
        /// * contentType
        /// 
        /// Then it needs either the ID...:
        /// * entityId
        /// 
        /// ...or it needs the position within the list:
        /// * useModuleList: true
        /// * sortOrder: [number] (important so it knows the position
        static member inline new'((buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( "new" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Open the edit-dialog for a new content-item.  
        /// * contentType
        /// 
        /// Then it needs either the ID...:
        /// * entityId
        /// 
        /// ...or it needs the position within the list:
        /// * useModuleList: true
        /// * sortOrder: [number] (important so it knows the position
        static member inline new'(buildParams: IBuildParameter seq) : IBuildInstruction = command.new' (buildParams, [] :> ICallParameter seq)
        /// Open the edit-dialog for a new content-item.  
        /// * contentType
        /// 
        /// Then it needs either the ID...:
        /// * entityId
        /// 
        /// ...or it needs the position within the list:
        /// * useModuleList: true
        /// * sortOrder: [number] (important so it knows the position
        static member inline new'(identifier, buildParams: IBuildParameter seq) : IBuildInstruction = command.new' (identifier, buildParams, [])
        /// Open the edit-dialog for a new content-item.  
        /// * contentType
        /// 
        /// Then it needs either the ID...:
        /// * entityId
        /// 
        /// ...or it needs the position within the list:
        /// * useModuleList: true
        /// * sortOrder: [number] (important so it knows the position
        static member inline new'(callParams: ICallParameter seq) : IBuildInstruction = command.new' ([], callParams)
        /// Open the edit-dialog for a new content-item.  
        /// * contentType
        /// 
        /// Then it needs either the ID...:
        /// * entityId
        /// 
        /// ...or it needs the position within the list:
        /// * useModuleList: true
        /// * sortOrder: [number] (important so it knows the position
        static member inline new' identifier : IBuildInstruction = command.new' (identifier, [], [])
        /// Open the edit-dialog for a new content-item.  
        /// * contentType
        /// 
        /// Then it needs either the ID...:
        /// * entityId
        /// 
        /// ...or it needs the position within the list:
        /// * useModuleList: true
        /// * sortOrder: [number] (important so it knows the position
        static member inline new'() : IBuildInstruction = command.new' ([], [])
        

        /// Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline add((identifier:string), (buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( identifier + "=add" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline add((buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( "add" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline add(buildParams: IBuildParameter seq) : IBuildInstruction = command.add (buildParams, [] :> ICallParameter seq)
        /// Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline add(identifier, buildParams: IBuildParameter seq) : IBuildInstruction = command.add (identifier, buildParams, [])
        /// Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline add(callParams: ICallParameter seq) : IBuildInstruction = command.add ([], callParams)
        /// Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline add identifier : IBuildInstruction = command.add (identifier, [], [])
        /// Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline add (sortOrder:int, buildParams: IBuildParameter seq) : IBuildInstruction = command.add (buildParams , [ cp.useModuleList; cp.sortOrder sortOrder ])
        /// Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline add (sortOrder:int) : IBuildInstruction = command.add ([] , [ cp.useModuleList; cp.sortOrder sortOrder ])
        /// Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline add() : IBuildInstruction = command.add ([], [])

        ///  Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        /// But to do this, it shows the user a list of existing items.
        static member inline addExisting((identifier:string), (buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( identifier + "=add-existing" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        ///  Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        /// But to do this, it shows the user a list of existing items.
        static member inline addExisting((buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( "add-existing" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        ///  Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        /// But to do this, it shows the user a list of existing items.
        static member inline addExisting(buildParams: IBuildParameter seq) : IBuildInstruction = command.addExisting (buildParams, [] :> ICallParameter seq)
        ///  Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        /// But to do this, it shows the user a list of existing items.
        static member inline addExisting(identifier, buildParams: IBuildParameter seq) : IBuildInstruction = command.addExisting (identifier, buildParams, [])
        ///  Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        /// But to do this, it shows the user a list of existing items.
        static member inline addExisting(callParams: ICallParameter seq) : IBuildInstruction = command.addExisting ([], callParams)
        ///  Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        /// But to do this, it shows the user a list of existing items.
        static member inline addExisting identifier : IBuildInstruction = command.addExisting (identifier, [], [])
        ///  Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        /// But to do this, it shows the user a list of existing items.
        static member inline addExisting() : IBuildInstruction = command.addExisting ([], [])

        ///  Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        /// But to do this, it shows the user a list of existing items.

        /// Opens the edit-dialog. If the item is module-content it may also open the presentation-item as well.
        /// 
        /// It needs either the ID...:
        /// * entityId
        ///
        /// ...or it needs the position within the list:
        ///
        /// * useModuleList: true
        /// * sortOrder: [number] (important so it knows the position)
        static member inline edit((identifier:string), (buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( identifier + "=edit" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        ///  Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        /// But to do this, it shows the user a list of existing items.

        /// Opens the edit-dialog. If the item is module-content it may also open the presentation-item as well.
        /// 
        /// It needs either the ID...:
        /// * entityId
        ///
        /// ...or it needs the position within the list:
        ///
        /// * useModuleList: true
        /// * sortOrder: [number] (important so it knows the position)
        static member inline edit((buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( "edit" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        ///  Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        /// But to do this, it shows the user a list of existing items.

        /// Opens the edit-dialog. If the item is module-content it may also open the presentation-item as well.
        /// 
        /// It needs either the ID...:
        /// * entityId
        ///
        /// ...or it needs the position within the list:
        ///
        /// * useModuleList: true
        /// * sortOrder: [number] (important so it knows the position)
        static member inline edit(buildParams: IBuildParameter seq) : IBuildInstruction = command.edit (buildParams, [] :> ICallParameter seq)
        ///  Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        /// But to do this, it shows the user a list of existing items.

        /// Opens the edit-dialog. If the item is module-content it may also open the presentation-item as well.
        /// 
        /// It needs either the ID...:
        /// * entityId
        ///
        /// ...or it needs the position within the list:
        ///
        /// * useModuleList: true
        /// * sortOrder: [number] (important so it knows the position)
        static member inline edit(identifier, buildParams: IBuildParameter seq) : IBuildInstruction = command.edit (identifier, buildParams, [])
        ///  Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        /// But to do this, it shows the user a list of existing items.

        /// Opens the edit-dialog. If the item is module-content it may also open the presentation-item as well.
        /// 
        /// It needs either the ID...:
        /// * entityId
        ///
        /// ...or it needs the position within the list:
        ///
        /// * useModuleList: true
        /// * sortOrder: [number] (important so it knows the position)
        static member inline edit(callParams: ICallParameter seq) : IBuildInstruction = command.edit ([], callParams)
        ///  Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        /// But to do this, it shows the user a list of existing items.

        /// Opens the edit-dialog. If the item is module-content it may also open the presentation-item as well.
        /// 
        /// It needs either the ID...:
        /// * entityId
        ///
        /// ...or it needs the position within the list:
        ///
        /// * useModuleList: true
        /// * sortOrder: [number] (important so it knows the position)
        static member inline edit identifier : IBuildInstruction = command.edit (identifier, [], [])
        ///  Adds a content-item to the current list of items, right below the item where it was clicked.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        /// But to do this, it shows the user a list of existing items.

        /// Opens the edit-dialog. If the item is module-content it may also open the presentation-item as well.
        /// 
        /// It needs either the ID...:
        /// * entityId
        ///
        /// ...or it needs the position within the list:
        ///
        /// * useModuleList: true
        /// * sortOrder: [number] (important so it knows the position)
        static member inline edit() : IBuildInstruction = command.edit ([], [])
        
        /// delete (not just remove) a content-item. Needs:
        /// * entityId
        /// * entityGuid
        /// * entityTitle
        static member inline delete((identifier:string), (buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( identifier + "=delete" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// delete (not just remove) a content-item. Needs:
        /// * entityId
        /// * entityGuid
        /// * entityTitle
        static member inline delete((buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( "delete" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// delete (not just remove) a content-item. Needs:
        /// * entityId
        /// * entityGuid
        /// * entityTitle
        static member inline delete(buildParams: IBuildParameter seq) : IBuildInstruction = command.delete (buildParams, [] :> ICallParameter seq)
        /// delete (not just remove) a content-item. Needs:
        /// * entityId
        /// * entityGuid
        /// * entityTitle
        static member inline delete(identifier, buildParams: IBuildParameter seq) : IBuildInstruction = command.delete (identifier, buildParams, [])
        /// delete (not just remove) a content-item. Needs:
        /// * entityId
        /// * entityGuid
        /// * entityTitle
        static member inline delete(callParams: ICallParameter seq) : IBuildInstruction = command.delete ([], callParams)
        /// delete (not just remove) a content-item. Needs:
        /// * entityId
        /// * entityGuid
        /// * entityTitle
        static member inline delete identifier : IBuildInstruction = command.delete (identifier, [], [])
        /// delete (not just remove) a content-item. Needs:
        /// * entityId
        /// * entityGuid
        /// * entityTitle
        static member inline delete() : IBuildInstruction = command.delete ([], [])

        /// Removes an item from a list of items.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline remove((identifier:string), (buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( identifier + "=remove" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Removes an item from a list of items.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline remove((buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( "remove" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Removes an item from a list of items.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline remove(buildParams: IBuildParameter seq) : IBuildInstruction = command.remove (buildParams, [] :> ICallParameter seq)
        /// Removes an item from a list of items.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline remove(identifier, buildParams: IBuildParameter seq) : IBuildInstruction = command.remove (identifier, buildParams, [])
        /// Removes an item from a list of items.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline remove(callParams: ICallParameter seq) : IBuildInstruction = command.remove ([], callParams)
        /// Removes an item from a list of items.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline remove identifier : IBuildInstruction = command.remove (identifier, [], [])
        /// Removes an item from a list of items.
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)
        static member inline remove() : IBuildInstruction = command.remove ([], [])
        
        /// Move a content-item up one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        
        static member inline moveup((identifier:string), (buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( identifier + "=moveup" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Move a content-item up one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        
        static member inline moveup((buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( "moveup" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Move a content-item up one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        
        static member inline moveup(buildParams: IBuildParameter seq) : IBuildInstruction = command.moveup (buildParams, [] :> ICallParameter seq)
        /// Move a content-item up one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        
        static member inline moveup(identifier, buildParams: IBuildParameter seq) : IBuildInstruction = command.moveup (identifier, buildParams, [])
        /// Move a content-item up one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        
        static member inline moveup(callParams: ICallParameter seq) : IBuildInstruction = command.moveup ([], callParams)
        /// Move a content-item up one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        
        static member inline moveup identifier : IBuildInstruction = command.moveup (identifier, [], [])
        /// Move a content-item up one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        
        static member inline moveup() : IBuildInstruction = command.moveup ([], [])

        /// Move a content-item down one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        
        static member inline movedown((identifier:string), (buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( identifier + "=movedown" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Move a content-item down one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        
        static member inline movedown((buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( "movedown" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Move a content-item down one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        
        static member inline movedown(buildParams: IBuildParameter seq) : IBuildInstruction = command.movedown (buildParams, [] :> ICallParameter seq)
        /// Move a content-item down one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        
        static member inline movedown(identifier, buildParams: IBuildParameter seq) : IBuildInstruction = command.movedown (identifier, buildParams, [])
        /// Move a content-item down one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        
        static member inline movedown(callParams: ICallParameter seq) : IBuildInstruction = command.movedown ([], callParams)
        /// Move a content-item down one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        
        static member inline movedown identifier : IBuildInstruction = command.movedown (identifier, [], [])
        /// Move a content-item down one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        
        static member inline movedown() : IBuildInstruction = command.movedown ([], [])
        /// Move a content-item down one position in the list
        /// * useModuleList: true (required to be true for it to work)
        /// * sortOrder: [number] (important so it knows the position)        

        /// Open a dialog to manually re-order items in a list.
        /// (note: in older versions was called "sort")        
        static member inline instanceList((identifier:string), (buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( identifier + "=instance-list" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Open a dialog to manually re-order items in a list.
        /// (note: in older versions was called "sort")        
        static member inline instanceList((buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( "instance-list" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Open a dialog to manually re-order items in a list.
        /// (note: in older versions was called "sort")        
        static member inline instanceList(buildParams: IBuildParameter seq) : IBuildInstruction = command.instanceList (buildParams, [] :> ICallParameter seq)
        /// Open a dialog to manually re-order items in a list.
        /// (note: in older versions was called "sort")        
        static member inline instanceList(identifier, buildParams: IBuildParameter seq) : IBuildInstruction = command.instanceList (identifier, buildParams, [])
        /// Open a dialog to manually re-order items in a list.
        /// (note: in older versions was called "sort")        
        static member inline instanceList(callParams: ICallParameter seq) : IBuildInstruction = command.instanceList ([], callParams)
        /// Open a dialog to manually re-order items in a list.
        /// (note: in older versions was called "sort")        
        static member inline instanceList identifier : IBuildInstruction = command.instanceList (identifier, [], [])
        /// Open a dialog to manually re-order items in a list.
        /// (note: in older versions was called "sort")        
        static member inline instanceList() : IBuildInstruction = command.instanceList ([], [])

        /// Tells the system to update a content-items status to published. If there was a published and a draft before, the draft will replace the previous item. 
        static member inline publish((identifier:string), (buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( identifier + "=publish" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Tells the system to update a content-items status to published. If there was a published and a draft before, the draft will replace the previous item. 
        static member inline publish((buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( "publish" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Tells the system to update a content-items status to published. If there was a published and a draft before, the draft will replace the previous item. 
        static member inline publish(buildParams: IBuildParameter seq) : IBuildInstruction = command.publish (buildParams, [] :> ICallParameter seq)
        /// Tells the system to update a content-items status to published. If there was a published and a draft before, the draft will replace the previous item. 
        static member inline publish(identifier, buildParams: IBuildParameter seq) : IBuildInstruction = command.publish (identifier, buildParams, [])
        /// Tells the system to update a content-items status to published. If there was a published and a draft before, the draft will replace the previous item. 
        static member inline publish(callParams: ICallParameter seq) : IBuildInstruction = command.publish ([], callParams)
        /// Tells the system to update a content-items status to published. If there was a published and a draft before, the draft will replace the previous item. 
        static member inline publish identifier : IBuildInstruction = command.publish (identifier, [], [])
        /// Tells the system to update a content-items status to published. If there was a published and a draft before, the draft will replace the previous item. 
        static member inline publish() : IBuildInstruction = command.publish ([],[])

        /// Only available on module-assigned content items. Will open the dialog to assign a different content-item in this slot.
        static member inline replace((identifier:string), (buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( identifier + "=replace" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Only available on module-assigned content items. Will open the dialog to assign a different content-item in this slot.
        static member inline replace((buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( "replace" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Only available on module-assigned content items. Will open the dialog to assign a different content-item in this slot.
        static member inline replace(buildParams: IBuildParameter seq) : IBuildInstruction = command.replace (buildParams, [] :> ICallParameter seq)
        /// Only available on module-assigned content items. Will open the dialog to assign a different content-item in this slot.
        static member inline replace(identifier, buildParams: IBuildParameter seq) : IBuildInstruction = command.replace (identifier, buildParams, [])
        /// Only available on module-assigned content items. Will open the dialog to assign a different content-item in this slot.
        static member inline replace(callParams: ICallParameter seq) : IBuildInstruction = command.replace ([], callParams)
        /// Only available on module-assigned content items. Will open the dialog to assign a different content-item in this slot.
        static member inline replace identifier : IBuildInstruction = command.replace (identifier, [], [])
        /// Only available on module-assigned content items. Will open the dialog to assign a different content-item in this slot.
        static member inline replace() : IBuildInstruction = command.replace ([], [])

        /// Execute custom javascript
        /// * customCode - some JS like "alert('hello');"
        static member inline custom((identifier:string), (buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( identifier + "=custom" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Execute custom javascript
        /// * customCode - some JS like "alert('hello');"
        static member inline custom((buildParams: IBuildParameter seq), (callParams: ICallParameter seq)) : IBuildInstruction = unbox ( "custom" + Interop.mkBuildParams buildParams + Interop.mkCallParams callParams )
        /// Execute custom javascript
        /// * customCode - some JS like "alert('hello');"
        static member inline custom(buildParams: IBuildParameter seq) : IBuildInstruction = command.custom (buildParams, [] :> ICallParameter seq)
        /// Execute custom javascript
        /// * customCode - some JS like "alert('hello');"
        static member inline custom(identifier, buildParams: IBuildParameter seq) : IBuildInstruction = command.custom (identifier, buildParams, [])
        /// Execute custom javascript
        /// * customCode - some JS like "alert('hello');"
        static member inline custom(callParams: ICallParameter seq) : IBuildInstruction = command.custom ([], callParams)
        /// Execute custom javascript
        /// * customCode - some JS like "alert('hello');"
        static member inline custom identifier : IBuildInstruction = command.custom (identifier, [], [])
        /// Execute custom javascript
        /// * customCode - some JS like "alert('hello');"
        static member inline custom() : IBuildInstruction = command.custom ([], [])
    
    /// To modify a default thing - like change a the color of the new-button
    let modify (bi: IBuildInstruction) : IBuildInstruction = unbox ("%" + unbox (bi))
    /// To add a button or group (this is the default)
    let add (bi: IBuildInstruction) : IBuildInstruction = unbox ("+" + unbox (bi))
    /// To remove a button or group from the list
    let remove (bi: IBuildInstruction) : IBuildInstruction = unbox ("-" + unbox (bi))
    ///  a comment - like "here comes a special add-button"
    let comment (comment: string) : IBuildInstruction = unbox ("/" + comment)

[<RequireQualifiedAccess>]
module Sxc =
    /// Expects a sequense of Build Instructions (bi.xxxx)
    let inline toolbar (bi: IBuildInstruction seq) =
        bi
        |> Seq.map (fun s -> "\"" + (unbox s) + "\"")
        |> fun sx -> "[" + String.Join(", ", sx) + "]"

[<RequireQualifiedAccess>]
module prop =
    /// Adds atribute "sxc-toolbar". Use the *Sxc.toolbar* Builder to generate the required string.
    let inline toolbar (toolbar:string) = 
        Feliz.prop.custom ("sxc-toolbar", toolbar)

open Feliz
open SxcContext
open Feliz.UseListener
[<RequireQualifiedAccess>]
module React =
    /// Starts the 2sxc Toolbar Manager for the element 
    /// * The _refresh_ callback is executed when a triggered 2sxc Dialog is closed again. 
    let useToolbarRef (sxc: Sxc.Context) (refresh: unit -> unit) =
        let elemRef = React.useElementRef ()

        if sxc.Instance.isEditMode () then
            React.useLayoutEffect (fun () -> elemRef.current |> Option.iter sxc.Root.SetupToolbar)
            React.useElementListener.on (elemRef, "toolbar-init", sxc.Root.OnRefresh refresh)

        elemRef        