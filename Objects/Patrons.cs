using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace LibraryNameSpace
{
  public class Patron
  {
    private int _id;
    private string _name;

    public Patron(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }
    public override bool Equals(System.Object otherPatron)
    {
      if (!(otherPatron is Patron))
      {
        return false;
      }
      else {
        Patron newPatron = (Patron) otherPatron;
        bool idEquality = this.GetId() == newPatron.GetId();
        bool nameEquality = this.GetTitle() == newPatron.GetTitle();
        return (idEquality && nameEquality);
      }
  }
  public int GetId()
  {
    return _id;
  }
  public string GetTitle()
  {
    return _name;
  }


  public static List<Patron> GetAll()
  {
    List<Patron> AllPatrons = new List<Patron>{};

    SqlConnection conn = DB.Connection();
    SqlDataReader rdr = null;
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT * FROM patrons", conn);
    rdr = cmd.ExecuteReader();

    while(rdr.Read())
    {
      int patronId = rdr.GetInt32(0);
      string patronTitle = rdr.GetString(1);
      Patron newPatron = new Patron(patronTitle, patronId);
      AllPatrons.Add(newPatron);
    }
    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }
    return AllPatrons;
  }

  public static List<Patron> SearchPatrons(string searchPatrons)
  {
    SqlConnection conn = DB.Connection();
    SqlDataReader rdr = null;
    conn.Open();

    List<Patron> patrons = new List<Patron>{};

    SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE name = @search", conn);

    SqlParameter PatronIdParameter = new SqlParameter();
    PatronIdParameter.ParameterName = "@search";
    PatronIdParameter.Value = searchPatrons;

    cmd.Parameters.Add(PatronIdParameter);

    rdr = cmd.ExecuteReader();

    List<int> patronsIds = new List<int> {};

    int searchedPatronId = 0;
    string searchedTitle = null;



    while(rdr.Read())
    {
      searchedPatronId = rdr.GetInt32(0);
      searchedTitle = rdr.GetString(1);

      Patron newPatron = new Patron(searchedTitle, searchedPatronId);
      patrons.Add(newPatron);
    }

    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }

    return patrons;
  }

  public void Save()
  {
    SqlConnection conn = DB.Connection();
    SqlDataReader rdr;
    conn.Open();

    SqlCommand cmd = new SqlCommand("INSERT INTO patrons (name) OUTPUT INSERTED.id VALUES (@PatronTitle)", conn);

    SqlParameter nameParameter = new SqlParameter();
    nameParameter.ParameterName = "@PatronTitle";
    nameParameter.Value = this.GetTitle();

    cmd.Parameters.Add(nameParameter);

    rdr = cmd.ExecuteReader();

    while(rdr.Read())
    {
      this._id = rdr.GetInt32(0);
    }
    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }
  }

  public void AddBook(Book newBook)
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("INSERT INTO books_authors (author_id, book_id) VALUES (@PatronId, @BookId)", conn);

    SqlParameter AuthorIdParameter = new SqlParameter();
    AuthorIdParameter.ParameterName = "@PatronId";
    AuthorIdParameter.Value = this.GetId();
    cmd.Parameters.Add(AuthorIdParameter);

    SqlParameter booksIdParameter = new SqlParameter();
    booksIdParameter.ParameterName = "@BookId";
    booksIdParameter.Value = newBook.GetId();
    cmd.Parameters.Add(booksIdParameter);

    cmd.ExecuteNonQuery();

    if (conn != null)
    {
      conn.Close();
    }
  }

  public void Update(string newPatron_Title)
  {
    SqlConnection conn = DB.Connection();
    SqlDataReader rdr;
    conn.Open();

    SqlCommand cmd = new SqlCommand("UPDATE patrons SET name = @Title OUTPUT INSERTED.name WHERE id = @PatronId;", conn);

    SqlParameter TitleParameter = new SqlParameter();
    TitleParameter.ParameterName = "@Title";
    TitleParameter.Value = newPatron_Title;

    SqlParameter PatronIdParameter = new SqlParameter();
    PatronIdParameter.ParameterName = "@PatronId";
    PatronIdParameter.Value = this.GetId();

    cmd.Parameters.Add(TitleParameter);
    cmd.Parameters.Add(PatronIdParameter);

    rdr = cmd.ExecuteReader();

    while(rdr.Read())
    {
      this._name = rdr.GetString(0);
    }

    if (rdr != null)
    {
      rdr.Close();
    }

    if (conn != null)
    {
      conn.Close();
    }
  }

  public static void DeleteAll()
  {
    SqlConnection conn = DB.Connection();
    conn.Open();
    SqlCommand cmd = new SqlCommand("DELETE FROM patrons;", conn);
    cmd.ExecuteNonQuery();
  }

  public static Patron Find(int id)
  {
    SqlConnection conn = DB.Connection();
    SqlDataReader rdr = null;
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE id = @PatronId", conn);
    SqlParameter patronIdParameter = new SqlParameter();
    patronIdParameter.ParameterName = "@PatronId";
    patronIdParameter.Value = id.ToString();
    cmd.Parameters.Add(patronIdParameter);
    rdr = cmd.ExecuteReader();

    int foundPatronId = 0;
    string foundPatronTitle = null;


    while(rdr.Read())
    {
      foundPatronId = rdr.GetInt32(0);
      foundPatronTitle = rdr.GetString(1);
    }
    Patron foundPatron = new Patron(foundPatronTitle, foundPatronId);

    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }
    return foundPatron;
  }

  public static Patron FindTitle(string name)
  {
    SqlConnection conn = DB.Connection();
    SqlDataReader rdr = null;
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE name = @Patronname", conn);
    SqlParameter patronIdParameter = new SqlParameter();
    patronIdParameter.ParameterName = "@Patronname";
    patronIdParameter.Value = name.ToString();

    cmd.Parameters.Add(patronIdParameter);
    rdr = cmd.ExecuteReader();

    int foundPatronId = 0;
    string foundPatronTitle = null;

    while(rdr.Read())
    {
      foundPatronId = rdr.GetInt32(0);
      foundPatronTitle = rdr.GetString(1);

    }
    Patron foundPatron = new Patron(foundPatronTitle, foundPatronId);

    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }
    return foundPatron;
  }

  public void AddPatron(Patron newPatron)
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("INSERT INTO books_patrons (book_id, patron_id, date_time) VALUES (@Book_id, @Patron_id, @DateTimeId);", conn);

    SqlParameter authorIdParameter = new SqlParameter();
    authorIdParameter.ParameterName = "@Patron_id";
    authorIdParameter.Value = newPatron.GetId();
    cmd.Parameters.Add(authorIdParameter);

    SqlParameter patronIdParameter = new SqlParameter();
    patronIdParameter.ParameterName = "@Patron_id";
    patronIdParameter.Value = this.GetId();
    cmd.Parameters.Add(patronIdParameter);

    SqlParameter DateTimeIdParameter = new SqlParameter();
    DateTimeIdParameter.ParameterName = "@DateTimeId";
    DateTimeIdParameter.Value = this.GetId();
    cmd.Parameters.Add(DateTimeIdParameter);

    cmd.ExecuteNonQuery();

    if (conn != null)
    {
      conn.Close();
    }
  }

  public List<Book> GetPatronBooks()
  {
    SqlConnection conn = DB.Connection();
    SqlDataReader rdr = null;
    conn.Open();

    List<Book> books = new List<Book>{};

    SqlCommand cmd = new SqlCommand("SELECT books.* FROM patrons JOIN books_patrons on (patrons.id = books_patrons.book_id) JOIN books on (books.id = books_patrons.book_id) WHERE patrons.id = @PatronId", conn);

    SqlParameter PatronIdParameter = new SqlParameter();
    PatronIdParameter.ParameterName = "@PatronId";
    PatronIdParameter.Value = this.GetId();

    cmd.Parameters.Add(PatronIdParameter);

    rdr = cmd.ExecuteReader();

    List<int> booksIds = new List<int> {};
    while(rdr.Read())
    {
      int booksId = rdr.GetInt32(0);
      string booksName = rdr.GetString(1);
      bool Checked_out = rdr.GetBoolean(2);

      Book newBook = new Book(booksName, Checked_out, booksId);
      books.Add(newBook);
    }
    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }


    return books;
  }

  public void Delete()
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("DELETE FROM patrons WHERE id = @Patron_id; DELETE FROM books_patrons WHERE patron_id = @Patron_id", conn);

    SqlParameter patronIdParameter = new SqlParameter();
    patronIdParameter.ParameterName = "@Patron_id";
    patronIdParameter.Value = this.GetId();

    cmd.Parameters.Add(patronIdParameter);
    cmd.ExecuteNonQuery();

    if (conn != null)
    {
      conn.Close();
    }
  }
 }
}
