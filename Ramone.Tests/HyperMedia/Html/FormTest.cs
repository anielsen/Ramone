﻿using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.MediaTypes.Html;
using Ramone.Tests.Common;
using System.Collections.Generic;


namespace Ramone.Tests.HyperMedia.Html
{
  [TestFixture]
  public class FormTest : TestHelper
  {
    protected override void TestFixtureSetUp()
    {
      base.TestFixtureSetUp();
      TestService.CodecManager.AddFormUrlEncoded<FormArgs>();
    }


    [Test]
    public void WhenSubmittingFormItIncludesDefaultValues_Keyed(
      [Values("multipart", "urlencoded")] string encType)
    {
      // Act
      IKeyValueForm form = GetForm(encType: encType);
      form.Value("Unused", "---");
      FormArgs result = form.Bind().Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("text", result.InputText);
      Assert.AreEqual("password", result.InputPassword);
      Assert.AreEqual("checkbox", result.InputCheckbox);
      Assert.AreEqual("hidden", result.InputHidden);
      Assert.AreEqual("textarea", result.TextArea);
      Assert.AreEqual("2", result.Select);
      Assert.AreEqual("1b", result.Radio1);
      Assert.IsEmpty(result.Radio2);
      //Assert.AreEqual("B,C", result.MultiSelectValue);
      Assert.AreEqual(encType, result.EncType);
    }


    [Test]
    public void WhenSubmittingFormItOverridesDefaultValues_Keyed(
      [Values("multipart", "urlencoded")] string encType)
    {
      // Act
      IKeyValueForm form = GetForm(encType: encType);
      form.Value("InputText", "abc");
      form.Value("InputPassword", "1234");
      form.Value("InputCheckbox", "not");
      form.Value("TextArea", "qwe");
      form.Value("Select", "1");
      form.Value("Radio1", "1a");
      form.Value("Radio2", "2b");
      FormArgs result = form.Bind().Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("abc", result.InputText);
      Assert.AreEqual("1234", result.InputPassword);
      Assert.AreEqual("not", result.InputCheckbox);
      Assert.AreEqual("hidden", result.InputHidden);
      Assert.AreEqual("qwe", result.TextArea);
      Assert.AreEqual("1", result.Select);
      Assert.AreEqual("1a", result.Radio1);
      Assert.AreEqual("2b", result.Radio2);
      //Assert.AreEqual("A,D", result.MultiSelectValue);
      Assert.AreEqual(encType, result.EncType);
    }


    [Test]
    public void WhenSubmittingFormItIncludesDefaultValues_Typed(
      [Values("multipart", "urlencoded")] string encType)
    {
      // Arrange
      FormArgs args = new FormArgs();

      //Session.SerializerSettings.

      // Act
      IKeyValueForm form = GetForm(encType: encType);
      FormArgs result = form.Value(args).Bind().Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("text", result.InputText);
      Assert.AreEqual("password", result.InputPassword);
      Assert.AreEqual("checkbox", result.InputCheckbox);
      Assert.AreEqual("hidden", result.InputHidden);
      Assert.AreEqual("textarea", result.TextArea);
      Assert.AreEqual("2", result.Select);
      Assert.AreEqual("1b", result.Radio1);
      Assert.IsEmpty(result.Radio2);
      //Assert.AreEqual("B,C", result.MultiSelectValue);
      Assert.AreEqual(encType, result.EncType);
    }


    [Test]
    public void WhenSubmittingFormItOverridesDefaultValues_Typed(
      [Values("multipart", "urlencoded")] string encType)
    {
      // Arrange
      FormArgs args = new FormArgs
      {
        InputText = "abc",
        InputPassword = "1234",
        InputCheckbox = "not",
        TextArea = "qwe",
        Select = "3",
        Radio1 = "1a",
        Radio2 = "2b"
        //MultiSelect = new List<string> { "A", "D" }
      };

      // Act
      IKeyValueForm form = GetForm(encType: encType);
      FormArgs result = form.Value(args).Bind().Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("abc", result.InputText);
      Assert.AreEqual("1234", result.InputPassword);
      Assert.AreEqual("not", result.InputCheckbox);
      Assert.AreEqual("hidden", result.InputHidden);
      Assert.AreEqual("qwe", result.TextArea);
      Assert.AreEqual("3", result.Select);
      Assert.AreEqual("1a", result.Radio1);
      Assert.AreEqual("2b", result.Radio2);
      //Assert.AreEqual("A,D", result.MultiSelectValue);
      Assert.AreEqual(encType, result.EncType);
    }


    [Test]
    public void WhenSubmittingWithoutNameItIncludesValuesFromFirstSubmitButton()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm();
      FormArgs result = form.Value(args).Bind().Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("Save", result.Save);
      Assert.IsEmpty(result.Cancel);
      Assert.IsEmpty(result.Help);
    }


    [Test]
    public void WhenSubmittingByNameItOnlyIncludesValuesFromTheSubmitButtonUsed()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm();
      FormArgs result = form.Value(args).Bind("Cancel").Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.IsEmpty(result.Save);
      Assert.AreEqual("Cancel", result.Cancel);
      Assert.IsEmpty(result.Help);
    }


    [Test]
    public void WhenSubmittingByIdItOnlyIncludesValuesFromTheSubmitButtonUsed()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm();
      FormArgs result = form.Value(args).Bind("#help-button").Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.IsEmpty(result.Save);
      Assert.IsEmpty(result.Cancel);
      Assert.AreEqual("Help", result.Help);
    }


    [Test]
    public void CanSubmitToRelativeActionUrl()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm("relative");
      FormArgs result = form.Value(args).Bind("Cancel").Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("Cancel", result.Cancel);
    }


    [Test]
    public void WhenNoActionUrlIsSetItSubmitsToCurrentUrl()
    {
      // Arrange
      FormArgs args = new FormArgs();

      // Act
      IKeyValueForm form = GetForm("empty");
      FormArgs result = form.Value(args).Bind("Cancel").Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("Cancel", result.Cancel);
    }


    [Test]
    public void WhenNoAcceptCharsetFoundItUsesCharsetFromPreviousResponse_Typed(
      [Values("iso-8859-1", "utf-8")] string charset)
    {
      // Arrange
      FormArgs args = new FormArgs
      {
        InputText = "ÆØÅüì"
      };

      // Act
      IKeyValueForm form = GetForm(charset: charset);
      FormArgs result = form.Value(args).Bind().Submit<FormArgs>().Body;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("ÆØÅüì", result.InputText);
      Assert.AreEqual(charset, result.Charset);
    }


    IKeyValueForm GetForm(string actionUrlMode = "absolute", string encType = "multipart", string charset = "iso-8859-1")
    {
      RamoneRequest formRequest = Session.Bind(FormTemplate, new { actionUrlMode = actionUrlMode, encType = encType, charset = charset });
      RamoneResponse<HtmlDocument> response = formRequest.Get<HtmlDocument>();
      IKeyValueForm form = response.Body.DocumentNode.SelectNodes(@"//form").First().Form(response);
      return form;
    }
  }
}
