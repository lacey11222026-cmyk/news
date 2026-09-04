/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.htmlEncodeOutput = true;
    config.language = 'vi',
    //config.enterMode = CKEDITOR.ENTER_BR;
    config.disableAutoInline = true;
    config.entities_latin = false;
    config.extraAllowedContent = 'iframe[*]';
    config.removePlugins = 'iframe';

    //config.pasteFromWordRemoveFontStyles = true;
    //config.pasteFromWordRemoveStyles = true;
    config.resize_enabled = true;
    //config.extraPlugins = 'tableresize';
    // ALLOW <i></i>
    //config.protectedSource.push(/<i[\s\S]*?\>/g); //allows beginning <i> tag
    //config.protectedSource.push(/<\/i[\s\S]*?\>/g); //allows ending </i> tag
    config.ignoreEmptyParagraph = true;
    config.enterMode = CKEDITOR.ENTER_BR;
    config.pasteFromWordRemoveFontStyles = false;
    config.pasteFromWordRemoveStyles = false;
};
