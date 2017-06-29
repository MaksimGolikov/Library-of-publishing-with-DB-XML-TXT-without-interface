using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library_of_books.Model;
using Library_of_books;

namespace Library_of_books.Presenter
{
    class MainPresenter
    {
        private MainFormFunction formModelBook;
        private MagazineFunction formModelMagazine;
        private NewspaperFunction formModelNewspaper;
        private fmMain formView;

        string incorrectCharacter = " ,.!@#$%^&*()_-+=<>";

        public MainPresenter(fmMain mainFormView)
        {
            formView = mainFormView;

            formModelBook = new MainFormFunction();
            formModelMagazine = new MagazineFunction();
            formModelNewspaper = new NewspaperFunction();         
        }
        


        public void GetAllBooks()
        {
            var loadedData = formModelBook.LoadBooks();
            formView.SetLoadedPublish(loadedData);
        }
        public void GetAllMagazine()
        {
            var loadedMagazine = new List<Book>();
            loadedMagazine.AddRange(formModelMagazine.LoadMagazines());           
            formView.SetLoadedPublish( loadedMagazine );
        }
        public void GetAllPublish()
        {
            var loadedBooks = formModelBook.LoadBooks();               
            loadedBooks.AddRange(formModelMagazine.LoadMagazines());
            loadedBooks.AddRange(formModelNewspaper.LoadNewspaper());

            formView.SetLoadedPublish(loadedBooks);
        }
        public void GetAllNewspaper()
        {
            var loadedNewspaper = new List<Book>();
            loadedNewspaper.AddRange(formModelNewspaper.LoadNewspaper());
            formView.SetLoadedPublish(loadedNewspaper);
        }
        public void GetSelectedBook(int idPublish, string typePublish)
        {
            bool used = false;

            if (typePublish == "Book")
            {
                var selectedBook = formModelBook.SetSelectedBook(idPublish);
                formView.SetSelectedPublish(selectedBook);
                used = true;
            }            
            if (!used && typePublish == "Newspaper")
            {
                var selectedPublish = formModelNewspaper.SetSelectedMagazine(idPublish);
                formView.SetSelectedPublish(selectedPublish);
                used = true;
            }
            if (!used && typePublish == "Magazine")
            {
                var selectedPublish = formModelMagazine.SetSelectedMagazine(idPublish);
                formView.SetSelectedPublish(selectedPublish);
            }           
        }
        public void GetSelectedArticle(int idPublish, int idArticle,string typePublish)
        {
            if (typePublish =="Magazine")
            {
                var selectedBook = formModelMagazine.GetSelectedArticle(idPublish, idArticle);
                formView.SetSelectedPublish(selectedBook);
            }
            if (typePublish == "Newspaper")
            {
                var selectedBook = formModelNewspaper.GetSelectedArticle(idPublish, idArticle);
                formView.SetSelectedPublish(selectedBook);
            }
                  
        }



        public void SearchByName(string name)
        {
            var resaltSearch = formModelBook.SearchByName(name);
            resaltSearch.AddRange(formModelMagazine.SearchByName(name));
            resaltSearch.AddRange(formModelNewspaper.SearchByName(name));

            formView.SetLoadedPublish(resaltSearch);
        }
        public void SearchByAuthor(string author)
        {
            var resaltSearch = formModelBook.SearchByAuthor(author);           
            formView.SetLoadedPublish(resaltSearch);
        }
        public void SearchByPublishingHouse(string publishingHouse)
        {
            var resaltSearch = formModelBook.SearchByPublishingHouse(publishingHouse);         
            formView.SetLoadedPublish(resaltSearch);
        }
        public void SearchByPublishingYear(string publishingYear)
        {
            var resaltSearch = formModelBook.SearchByPublishingYear(publishingYear);
            resaltSearch.AddRange(formModelMagazine.SearchByPublishingYear(publishingYear));
            resaltSearch.AddRange(formModelNewspaper.SearchByPublishingYear(publishingYear));

            formView.SetLoadedPublish(resaltSearch);
        }
        public void SearchByRelease(int release)
        {
            var resaltSearch = new List<Book>();
            resaltSearch.AddRange(formModelMagazine.SearchByRelease(release));
            resaltSearch.AddRange(formModelMagazine.SearchByRelease(release));
            formView.SetLoadedPublish(resaltSearch);
        }



        public string CreateNewArticle(int idMagazine, Book book, string typePublish)
        {
            var returnText = "Article successful added";
            var shouldAdd = IsArticleCorrect(idMagazine, book,typePublish);

            if (typePublish =="Magazine")
            {
                if (shouldAdd)
                {
                    formModelMagazine.CreateNewArticle(idMagazine, book);

                    var selectedPublish = formModelMagazine.SetSelectedMagazine(idMagazine);
                    formView.SetSelectedPublish(selectedPublish);
                }
                if (!shouldAdd)
                {
                    returnText = "You entered incorrect information. Book don`t added";
                }

                formModelMagazine.ChangeXMLFile();
            }
            if (typePublish != "Magazine")
            {
                if (shouldAdd)
                {
                    formModelNewspaper.CreateNewArticle(idMagazine, book);
                    var selectedPublish = formModelNewspaper.SetSelectedMagazine(idMagazine);
                    formView.SetSelectedPublish(selectedPublish);
                }
                if (!shouldAdd)
                {
                    returnText = "You entered incorrect information. Book don`t added";
                }

                formModelNewspaper.ChangeTXTfile();
            }


            
            return returnText;
        }
        public string CreateNewPublish(Magazine magazine)
        {
            var returnText = "Publish successful added";
            var shouldAdd = IsMagazineCorrect(magazine);

            if (shouldAdd)
            {
                formModelMagazine.CreateNewMagazine(magazine);

                var loadedMagazine = new List<Book>();
                loadedMagazine.AddRange(formModelMagazine.LoadMagazines());
                formView.SetLoadedPublish(loadedMagazine);
                formView.SetSelectedPublish(loadedMagazine[loadedMagazine.Count-1]);
                formModelMagazine.ChangeXMLFile();
            }
            if (!shouldAdd)
            {
                returnText = "You entered incorrect information. Publish don`t added";
            }

          
            return returnText;
        }
        public string CreateNewPublish(Newspaper magazine)
        {
            var returnText = "Publish successful added";
            var shouldAdd = IsNewspaperCorrect(magazine);

            if (shouldAdd)
            {
                formModelNewspaper.CreateNewMagazine(magazine);

                var loadedNewspaper = new List<Book>();
                loadedNewspaper.AddRange(formModelNewspaper.LoadNewspaper());
                formView.SetLoadedPublish(loadedNewspaper);
                formView.SetSelectedPublish(loadedNewspaper[loadedNewspaper.Count - 1]);
                formModelNewspaper.ChangeTXTfile();
            }
            if (!shouldAdd)
            {
                returnText = "You entered incorrect information. Publish don`t added";
            }

          
            return returnText;
        }
        public string CreateNewBook(Book book)
        {
            var returnText = "Book successful added";
            var shouldAdd = IsBookCorrect(book);
            var quotationMark = "'";
            var testDescription = "";

            if (shouldAdd)
            {
                for(int i=0;i<book.Description.Length;i++)
                {
                    if(book.Description[i] == quotationMark[0])
                    {
                        testDescription += "_";
                        continue;
                    }
                    if (testDescription.Length < 255)
                    {
                        testDescription += book.Description[i];
                    }
                    if (testDescription.Length >= 255)
                    {
                        break;
                    }
                }
                book.Description = testDescription;

                formModelBook.CreateNewBook(book);

                var loadedData = formModelBook.LoadBooks();
                formView.SetLoadedPublish(loadedData);

                formView.SetSelectedPublish(loadedData[loadedData.Count-1]);
            }
            if (!shouldAdd)
            {
                returnText = "You entered incorrect information. Book don`t added";
            }
           

            formModelMagazine.ChangeXMLFile();
            return returnText;
        }



        public void DeleteSelectedBook(int idBook)
        {
            formModelBook.DeleteBook(idBook);
            formModelMagazine.ChangeXMLFile();
        }
        public void DeleteSelectedArticle(int idPublish, int idArticle)
        {
            formModelMagazine.DeleteArticle(idPublish, idArticle);

            var selectedPublish = formModelMagazine.SetSelectedMagazine(idPublish);
            formView.SetSelectedPublish(selectedPublish);
            formModelMagazine.ChangeXMLFile();
        }
        public void DeleteSelectedPublish(int idPublish, string typePublish)
        {
            if (typePublish =="Magazine")
            {
                formModelMagazine.DeleteMagazine(idPublish);
                formModelMagazine.ChangeXMLFile();

                var loadedMagazine = new List<Book>();
                loadedMagazine.AddRange(formModelMagazine.LoadMagazines());
                formView.SetLoadedPublish(loadedMagazine);
               
            }
            if (typePublish == "Newspaper")
            {
                formModelNewspaper.DeleteMagazine(idPublish);
                var loadedMagazine = new List<Book>();
                loadedMagazine.AddRange(formModelNewspaper.LoadNewspaper());
                formView.SetLoadedPublish(loadedMagazine);
                formModelNewspaper.ChangeTXTfile();
            }

           
        }





        public string ChangeArticleInfo(int idMagazine, Book book, string typePublish)
        {
            var resaltText = "Information has been chenged";

            if (typePublish == "Magazine")
            {
                var shouldChange = IsArticleCorrect(idMagazine, book,typePublish);

                if (shouldChange)
                {
                    formModelMagazine.ChangeArticleInfo(idMagazine, book);
                }
                if (!shouldChange)
                {
                    resaltText = "Sending wronge, information hasn`t chenged";
                }
                formModelMagazine.ChangeXMLFile();
            }
            if (typePublish == "Newspaper")
            {
                var shouldChange = IsArticleCorrect(idMagazine, book, typePublish);

                if (shouldChange)
                {
                    formModelNewspaper.ChangeArticleInfo(idMagazine, book);
                }
                if (!shouldChange)
                {
                    resaltText = "Sending wronge, information hasn`t chenged";
                }
                formModelNewspaper.ChangeTXTfile();
            }



           
            return resaltText;
        }
        public string ChangePublisheInfo(Magazine magazine)
        {
            var resaltText = "Information has been chenged";
          
            formModelMagazine.ChangeMagazinInfo(magazine);

            formModelMagazine.ChangeXMLFile();
            return resaltText;
        }
        public string ChangePublisheInfo(Newspaper magazine)
        {
            var resaltText = "Information has been chenged";

            formModelNewspaper.ChangeMagazinInfo(magazine);

            formModelNewspaper.ChangeTXTfile();
            return resaltText;
        }
        public string ChangeBookInfo(Book book)
        {
            var resaltText = "Information has been chenged";
            var shouldChange = IsBookCorrect(book);

            if (shouldChange)
            {
                formModelBook.ChangeBookInfo(book);
            }
            if (!shouldChange)
            {
                resaltText = "Sending wronge, information hasn`t chenged";
            }
            var loadedData = formModelBook.LoadBooks();
            formView.SetLoadedPublish(loadedData);

            formModelMagazine.ChangeXMLFile();
            return resaltText;
        }
   


        private bool IsBookCorrect(Book book)
        {
            bool resalt = true;

            var loadedData = formModelBook.LoadBooks();
            foreach (Book b in loadedData)
            {
                if (book.NameBook == b.NameBook && book.Author == b.Author && book.PublishingHouse == b.PublishingHouse &&
                    book.PublishingYear == b.PublishingYear && book.Description == b.Description)
                {
                    resalt = false;
                    break;
                }
                if (book.NameBook == "" || book.Author == "" || book.PublishingHouse == "" ||
                    book.PublishingYear == "" || book.Description == "")
                {
                    resalt = false;
                    break;
                }
            }

            if (resalt)
            {
                if (book.NameBook.Length>30 || book.PublishingHouse.Length>20 || book.PublishingYear.Length>4)
                {
                    resalt = false;
                }
            }



            if(resalt)
            {
                foreach (char c in incorrectCharacter)
                {
                    if (book.NameBook == c.ToString() || book.Author == c.ToString() || book.PublishingHouse == c.ToString() ||
                        book.PublishingYear == c.ToString() || book.Description == c.ToString())
                    {
                        resalt = false;
                        break;
                    }
                }
            }
               

            return resalt;
        }
        private bool IsArticleCorrect(int idMagazine, Book book,string typePublsh)
        {
            bool resalt = true;

            if (typePublsh == "Magazine")
            {
                var loadedData = formModelMagazine.SetSelectedMagazine(idMagazine);

                foreach (Book b in loadedData.Article)
                {
                    if (book.NameBook == b.NameBook && book.Author == b.Author && book.PublishingHouse == b.PublishingHouse &&
                        book.PublishingYear == b.PublishingYear && book.Description == b.Description)
                    {
                        resalt = false;
                        break;
                    }
                    if (book.NameBook == "" || book.Author == "" || book.PublishingHouse == "" ||
                        book.PublishingYear == "" || book.Description == "")
                    {
                        resalt = false;
                        break;
                    }
                }
            }
            if (typePublsh != "Magazine")
            {
                var loadedData = formModelNewspaper.SetSelectedMagazine(idMagazine);

                foreach (Book b in loadedData.Article)
                {
                    if (book.NameBook == b.NameBook && book.Author == b.Author && book.PublishingHouse == b.PublishingHouse &&
                        book.PublishingYear == b.PublishingYear && book.Description == b.Description)
                    {
                        resalt = false;
                        break;
                    }
                    if (book.NameBook == "" || book.Author == "" || book.PublishingHouse == "" ||
                        book.PublishingYear == "" || book.Description == "")
                    {
                        resalt = false;
                        break;
                    }
                }
            }
            
           

            if (resalt)
            {
                foreach (char c in incorrectCharacter)
                {
                    if (book.NameBook == c.ToString() || book.Author == c.ToString() || book.PublishingHouse == c.ToString() ||
                        book.PublishingYear == c.ToString() || book.Description == c.ToString())
                    {
                        resalt = false;
                        break;
                    }
                }
            }


            return resalt;
        }
        private bool IsMagazineCorrect(Magazine magazine)
        {
            bool resalt = true;

            var loadedData = formModelMagazine.LoadMagazines();
            foreach (Magazine b in loadedData)
            {
                if (magazine.NameBook == b.NameBook && magazine.PublishingYear == b.PublishingYear && magazine.Description == b.Description)
                {
                    resalt = false;
                    break;
                }
                if (magazine.NameBook == "" || magazine.PublishingYear == "" || magazine.Description == "")
                {
                    resalt = false;
                    break;
                }
            }

            if (resalt)
            {
                foreach (char c in incorrectCharacter)
                {
                    if (magazine.NameBook == c.ToString() || magazine.PublishingYear == c.ToString() || magazine.Description == c.ToString())
                    {
                        resalt = false;
                        break;
                    }
                }
            }


            return resalt;
        }
        private bool IsNewspaperCorrect(Newspaper magazine)
        {
            bool resalt = true;

            var loadedData = formModelNewspaper.LoadNewspaper();
            foreach (Newspaper b in loadedData)
            {
                if (magazine.NameBook == b.NameBook && magazine.PublishingYear == b.PublishingYear && magazine.Description == b.Description)
                {
                    resalt = false;
                    break;
                }
                if (magazine.NameBook == "" || magazine.PublishingYear == "" || magazine.Description == "")
                {
                    resalt = false;
                    break;
                }
            }

            if (resalt)
            {
                foreach (char c in incorrectCharacter)
                {
                    if (magazine.NameBook == c.ToString() || magazine.PublishingYear == c.ToString() || magazine.Description == c.ToString())
                    {
                        resalt = false;
                        break;
                    }
                }
            }


            return resalt;
        }

    }
}
