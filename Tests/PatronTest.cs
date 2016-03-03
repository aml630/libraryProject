using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LibraryNameSpace
{
  public class PatronTest : IDisposable
  {
    public PatronTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }

    // [Fact]
    // public void Test_AddAuthor_AddsAuthorToPatron()
    // {
    //   //Arrange
    //   Patron testPatron = new Patron("James");
    //   testPatron.Save();
    //
    //   Author firstAuthor = new Author("Magic Johnson");
    //   firstAuthor.Save();
    //
    //   Author secondAuthor = new Author("Magic James");
    //   secondAuthor.Save();
    //
    //   //Act
    //   testPatron.AddAuthor(firstAuthor);
    //   testPatron.AddAuthor(secondAuthor);
    //
    //   List<Author> result = testPatron.GetAuthors();
    //   List<Author> testList = new List<Author>{firstAuthor, secondAuthor};
    //
    //   //Assert
    //   Assert.Equal(testList, result);
    // }

    // [Fact]
    // public void Test_GetAuthors_ReturnsAllPatronAuthors()
    // {
    //   //Arrange
    //   Patron testPatron = new Patron("James");
    //   testPatron.Save();
    //
    //   Author firstAuthor = new Author("Magic Johnson");
    //   firstAuthor.Save();
    //
    //   Author secondAuthor = new Author("Magic James");
    //   secondAuthor.Save();
    //
    //   //Act
    //   testPatron.AddAuthor(firstAuthor);
    //   List<Author> savedAuthors = testPatron.GetAuthors();
    //   List<Author> testList = new List<Author> {firstAuthor};
    //
    //   //Assert
    //   Assert.Equal(testList, savedAuthors);
    // }
    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Patron.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Patron testPatron = new Patron("James");

      Patron secondPatron = new Patron("James");

      //Assert
      Assert.Equal(testPatron, secondPatron);
    }

    [Fact]
    public void Test_Save_SavesPatronToDatabase()
    {
      //Arrange
      Patron testPatron = new Patron("James");
      testPatron.Save();

      //Act
      List<Patron> result = Patron.GetAll();
      List<Patron> testList = new List<Patron>{testPatron};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToPatronObject()
    {
      //Arrange
      Patron testPatron = new Patron("James");
      testPatron.Save();

      //Act
      Patron savedPatron = Patron.GetAll()[0];
      int result = savedPatron.GetId();
      int testId = testPatron.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsPatronInDatabase()
    {
      //Arrange
      Patron testPatron = new Patron("James");
      testPatron.Save();

      //Act
      Patron foundPatron = Patron.Find(testPatron.GetId());

      //Assert
      Assert.Equal(testPatron, foundPatron);
    }

    // [Fact]
    // public void Test_Find_PatronByTitle()
    // {
    //   //Arrange
    //   Patron testPatron = new Patron("James");
    //   testPatron.Save();
    //
    //   //Act
    //   Patron foundPatron = Patron.FindTitle(testPatron.GetTitle());
    //
    //   //Assert
    //   Assert.Equal(testPatron, foundPatron);
    // }

    [Fact]
    public void Test_Update_UpdatesPatronInDatabase()
    {
      //Arrange

      Patron testPatron = new Patron("James");
      testPatron.Save();
      string newName = "Work stuff";

      //Act
      testPatron.Update(newName);

      string result = testPatron.GetTitle();

      //Assert
      Assert.Equal(newName, result);
    }

    // [Fact]
    // public void Test_AddAuthor_AddsPatronToAuthor()
    // {
    //   //Arrange
    //   Patron testPatron = new Patron("James");
    //   testPatron.Save();
    //
    //   Author secondAuthor = new Author("Magic Johnson");
    //   secondAuthor.Save();
    //
    //   //Act
    //   testPatron.AddAuthor(secondAuthor);
    //
    //   List<Author> result = testPatron.GetAuthors();
    //   List<Author> testList = new List<Author>{secondAuthor};
    //
    //   //Assert
    //   Assert.Equal(testList, result);
    // }

    // [Fact]
    // public void Test_Delete_DeletesPatronAssociationsFromDatabase()
    // {
    //   //Arrange
    //   Author testAuthor = new Author("Magic Johnson");
    //   testAuthor.Save();
    //
    //   string testName = "Home stuff";
    //   Patron testPatron = new Patron(testName);
    //   testPatron.Save();
    //
    //   //Act
    //   testPatron.AddAuthor(testAuthor);
    //   testPatron.Delete();
    //
    //   List<Author> resultAuthorCategories = testPatron.GetAuthors();
    //   List<Author> testAuthorCategories = new List<Author> {};
    //
    //   //Assert
    //   Assert.Equal(testAuthorCategories, resultAuthorCategories);
    // }

    [Fact]
    public void Test_Delete_DeletesPatronFromDatabase()
    {
      //Arrange
      string name1 = "Soccer";
      Patron testPatron1 = new Patron(name1);
      testPatron1.Save();

      string name2 = "Dancing";
      Patron testPatron2 = new Patron(name2);
      testPatron2.Save();

      //Act
      testPatron1.Delete();
      List<Patron> resultCategories = Patron.GetAll();
      List<Patron> testPatronList = new List<Patron> {testPatron2};

      //Assert
      Assert.Equal(testPatronList, resultCategories);
    }

    // [Fact]
    // public void Test_Checked_Out_true()
    // {
    //   //Arrange
    //   Patron testPatron = new Patron("moby dick");
    //   testPatron.Save();
    //
    //   Patron testPatron2 = new Patron("moby dick");
    //   testPatron2.Save();
    //   testPatron2.CheckedOutUpdateTrue();
    //
    //   //Assert
    //   Assert.Equal(testPatron.GetCheckedOut(), testPatron2.GetCheckedOut());
    // }

    [Fact]
    public void Dispose()
    {
      Patron.DeleteAll();
    }
  }
}
