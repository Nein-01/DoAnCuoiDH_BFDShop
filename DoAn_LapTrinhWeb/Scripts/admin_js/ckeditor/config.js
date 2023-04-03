/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    //config.uiColor = 'rgb(245, 247, 255)';
    config.syntaxhighlight_lang = 'csharp';
    config.syntaxhighlight_hideControls = true;
    config.skin = 'office2013';
    config.languages = 'vi';
	config.filebrowserBrowseUrl = "/Scripts/admin_js/ckfinder/ckfinder.html";
	config.filebrowserImageUrl = "/Scripts/admin_js/ckfinder/ckfinder.html?Types=Images";
	config.filebrowserFlashUrl = "/Scripts/admin_js/ckfinder/ckfinder.html?Types=Flash";
    //config.filebrowserUploadUrl = "/Scripts/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=File";
	config.filebrowserImageUploadUrl = "/Scripts/admin_js/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images";
	config.filebrowserFlashUploadUrl = "/Scripts/admin_js/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash";
	config.toolbarGroups = [
		{ name: 'document', groups: ['mode', 'document', 'doctools'] },
		{ name: 'clipboard', groups: ['clipboard', 'undo'] },
		{ name: 'editing', groups: ['selection', 'find', 'spellchecker', 'editing'] },
		{ name: 'forms', groups: ['forms'] },
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
		{ name: 'links', groups: ['links'] },
		{ name: 'insert', groups: ['insert'] },
		{ name: 'styles', groups: ['styles'] },
		{ name: 'colors', groups: ['colors'] },
		{ name: 'tools', groups: ['tools'] },
		{ name: 'others', groups: ['others'] },
		{ name: 'about', groups: ['about'] }
	];
	//config.extraPlugins = 'exportpdf';
	config.extraPlugins = 'tableresize';
	config.stylesSet = 'styles.js';
	config.removeButtons = 'Save,Flash,About,Scayt,PageBreak,Anchor,Superscript,Subscript,Subscript';
};
