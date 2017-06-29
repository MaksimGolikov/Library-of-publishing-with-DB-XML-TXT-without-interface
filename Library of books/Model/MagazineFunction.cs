using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml.Serialization;


namespace Library_of_books.Model
{
    class MagazineFunction
    {

        List<Magazine> magazines;
        int nextID = 1;
        int nextRelease = 1;



        public MagazineFunction()
        {
            magazines = new List<Magazine>();

            StartConfiguraton();
        }


        


         public void CreateNewArticle(int magazineID, Book newArticlce)
        {
            for (int i = 0; i < magazines.Count; i++)
            {
                if (magazines[i].ID == magazineID)
                {
                    newArticlce.ID = magazines[i].NextArticleID;
                    magazines[i].Article.Add(newArticlce);
                    magazines[i].NextArticleID++;
                    break;
                }
            }
        }
         public void ChangeArticleInfo(int magazineID, Book article)
        {
            for (int i = 0; i < magazines.Count; i++)
            {
                if (magazines[i].ID == magazineID)
                {
                    for (int j = 0; j < magazines[i].Article.Count; j++)
                    {
                        if (magazines[i].Article[j].ID == article.ID)
                        {
                            magazines[i].Article[j] = article;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public void ChangeMagazinInfo(Magazine magazine)
        {
            for (int i = 0; i < magazines.Count; i++)
            {
                if (magazines[i].ID == magazine.ID)
                {                 
                    magazine.Article = magazines[i].Article;
                    magazines[i] = magazine;
                    break;
                }
            }
        }

        public void  CreateNewMagazine(Magazine magazine)
        {
            magazine.ID = nextID;         
            nextID++;

            magazines.Add(magazine);          
        }

        public void DeleteArticle(int idMagazine, int idArticle)
        {
            for (int i = 0; i < magazines.Count; i++)
            {
                if (magazines[i].ID == idMagazine)
                {
                    for(int j=0;j< magazines[i].Article.Count;j++)
                    {
                        if (magazines[i].Article[j].ID == idArticle)
                        {
                            magazines[i].Article.RemoveAt(j);
                            break;
                        }                       
                    }                   
                    break;
                }
            }                        
        }

        public void DeleteMagazine(int idMagazine)
        {
            for (int i=0;i<magazines.Count;i++)
            {
                if (magazines[i].ID == idMagazine)
                {
                    magazines.RemoveAt(i);
                    break;
                }
            }
        }

        public Book GetSelectedArticle(int idMagazine, int idArticle)
        {
            var returnedBook = new Book();
            for (int i = 0; i < magazines.Count; i++)
            {
                if (magazines[i].ID == idMagazine)
                {
                    for (int j = 0; j < magazines[i].Article.Count; j++)
                    {
                        if (magazines[i].Article[j].ID == idArticle)
                        {
                            returnedBook =  magazines[i].Article[j];
                            break;
                        }
                    }
                    break;
                }
            }
            return returnedBook;
        }

        public List<Magazine> LoadMagazines()
        {
            return magazines;
        }

        public List<Magazine> SearchByName(string name)
        {
            var returnedValue = magazines.FindAll(m => m.NameBook == name);
            return returnedValue;
        }

        public List<Magazine> SearchByPublishingYear(string publishingYear)
        {
            var returnedValue = magazines.FindAll(m => m.PublishingYear == publishingYear);
            return returnedValue;
        }

        public List<Magazine> SearchByRelease(int release)
        {
            var returnedValue = magazines.FindAll(m => m.Release == release);
            return returnedValue;
        }

        public Magazine SetSelectedMagazine(int idMagazine)
        {
            var returnedValue = magazines.Find(m => m.ID == idMagazine);
            return returnedValue;
        }




        public void ChangeXMLFile()
        {
            Stream stream = File.OpenWrite("Magazine.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(List<Magazine>));
            serializer.Serialize(stream,magazines);

            stream.Close();
        }        

        private void ReadXMLFile()
        {            
            Stream stream = File.OpenRead("Magazine.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(List<Magazine>));
            var read = serializer.Deserialize(stream);
            stream.Close();

            magazines = read as List<Magazine>;
            
        }

        private void StartConfiguraton()
        {
            bool existXMLfile = File.Exists("Magazine.xml");
            if (existXMLfile)
            {
                ReadXMLFile();
            }
           
        }
    }
}
