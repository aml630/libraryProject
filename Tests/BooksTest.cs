using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LibraryNameSpace
{
  public class BookTest : IDisposable
  {
    public BookTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }

    // [Fact]
    // public void Test_AddAuthor_AddsAuthorToBook()
    // {
    //   //Arrange
    //   Book testBook = new Book("Moby Dick");
    //   testBook.Save();
    //
    //   Author firstAuthor = new Author("Magic Johnson");
    //   firstAuthor.Save();
    //
    //   Author secondAuthor = new Author("Magic James");
    //   secondAuthor.Save();
    //
    //   //Act
    //   testBook.AddAuthor(firstAuthor);
    //   testBook.AddAuthor(secondAuthor);
    //
    //   List<Author> result = testBook.GetAuthors();
    //   List<Author> testList = new List<Author>{firstAuthor, secondAuthor};
    //
    //   //Assert
    //   Assert.Equal(testList, result);
    // }

    // [Fact]
    // public void Test_GetAuthors_ReturnsAllBookAuthors()
    // {
    //   //Arrange
    //   Book testBook = new Book("Moby Dick");
    //   testBook.Save();
    //
    //   Author firstAuthor = new Author("Magic Johnson");
    //   firstAuthor.Save();
    //
    //   Author secondAuthor = new Author("Magic James");
    //   secondAuthor.Save();
    //
    //   //Act
    //   testBook.AddAuthor(firstAuthor);
    //   List<Author> savedAuthors = testBook.GetAuthors();
    //   List<Author> testList = new List<Author> {firstAuthor};
    //
    //   //Assert
    //   Assert.Equal(testList, savedAuthors);
    // }
    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Book.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Book testBook = new Book("Moby Dick");

      Book secondBook = new Book("Moby Dick");

      //Assert
      Assert.Equal(testBook, secondBook);
    }

    [Fact]
    public void Test_Save_SavesBookToDatabase()
    {
      //Arrange
      Book testBook = new Book("Moby Dick");
      testBook.Save();

      //Act
      List<Book> result = Book.GetAll();
      List<Book> testList = new List<Book>{testBook};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToBookObject()
    {
      //Arrange
      Book testBook = new Book("Moby Dick");
      testBook.Save();

      //Act
      Book savedBook = Book.GetAll()[0];
      int result = savedBook.GetId();
      int testId = testBook.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsBookInDatabase()
    {
      //Arrange
      Book testBook = new Book("Moby Dick");
      testBook.Save();

      //Act
      Book foundBook = Book.Find(testBook.GetId());

      //Assert
      Assert.Equal(testBook, foundBook);
    }

    [Fact]
    public void Test_Update_UpdatesBookInDatabase()
    {
      //Arrange

      Book testBook = new Book("Moby Dick");
      testBook.Save();
      string newName = "Work stuff";

      //Act
      testBook.Update(newName);

      string result = testBook.GetTitle();

      //Assert
      Assert.Equal(newName, result);
    }

    [Fact]
    public void Test_AddAuthor_AddsBookToAuthor()
    {
      //Arrange
      Book testBook = new Book("Moby Dick");
      testBook.Save();

      Author secondAuthor = new Author("Magic Johnson");
      secondAuthor.Save();

      //Act
      testBook.AddAuthor(secondAuthor);

      List<Author> result = testBook.GetAuthors();
      List<Author> testList = new List<Author>{secondAuthor};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Delete_DeletesBookAssociationsFromDatabase()
    {
      //Arrange
      Author testAuthor = new Author("Magic Johnson");
      testAuthor.Save();

      string testName = "Home stuff";
      Book testBook = new Book(testName);
      testBook.Save();

      //Act
      testBook.AddAuthor(testAuthor);
      testBook.Delete();

      List<Author> resultAuthorCategories = testBook.GetAuthors();
      List<Author> testAuthorCategories = new List<Author> {};

      //Assert
      Assert.Equal(testAuthorCategories, resultAuthorCategories);
    }

    [Fact]
    public void Test_Delete_DeletesBookFromDatabase()
    {
      //Arrange
      string name1 = "Soccer";
      Book testBook1 = new Book(name1);
      testBook1.Save();

      string name2 = "Dancing";
      Book testBook2 = new Book(name2);
      testBook2.Save();

      //Act
      testBook1.Delete();
      List<Book> resultCategories = Book.GetAll();
      List<Book> testBookList = new List<Book> {testBook2};

      //Assert
      Assert.Equal(testBookList, resultCategories);
    }

    [Fact]
    public void Dispose()
    {
      Author.DeleteAll();
      Book.DeleteAll();
    }
  }
}
