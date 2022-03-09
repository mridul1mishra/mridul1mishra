Telerik.Web.UI.Editor.CommandList["InsertSitecoreVideo"] = function (commandName, editor, args) {
    var html = editor.getSelectionHtml();
    var id;

    // inserted media in form of <img src="-/media/CC2393E7CA004EADB4A155BE4761086B.ashx" />
   // if (!id) {
     //   id = GetMediaID(html);
    //}

    scEditor = editor;

    editor.showExternalDialog(
      "/sitecore/shell/default.aspx?xmlcontrol=RichText.InsertVideo&la=" + scLanguage + (id ? "&fo=" + id : "") + (scDatabase ? "&databasename=" + scDatabase : ""),
      null, //argument
      1105,
      700,
      scInsertSitecoreMedia,
      null,
      "Insert Media",
      true, //modal
      Telerik.Web.UI.WindowBehaviors.Close, // behaviors
      false, //showStatusBar
      false //showTitleBar
    );
};