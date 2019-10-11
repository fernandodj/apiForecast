using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ApiRest.Models;

namespace ApiRest
{
    public class ForecastJob : IJob
    {
        private ForecastEntities3 db = new ForecastEntities3();
        Task IJob.Execute(IJobExecutionContext context)
        {
            return Task.Run(() => {
                clearDatabase();
                calculateDays();
            });
            
        }

        public void clearDatabase()
        {
            Console.WriteLine("Eliminando registros anteriores...");
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE Climates");
        }

        public void calculateDays()
        {
            Point ferengiPos, vulcanoPos, betasoidePos, sunPosition;
            sunPosition = new Point();
            sunPosition.x = 0;
            sunPosition.y = 0;
            int days = 360, years = 10;
            
            string weather;

            Console.WriteLine("Generando nuevos registros....");
            for (int i = 0; i <= days * years; i++)
            {
                Climates climates = new Climates();
                ferengiPos = calculatePlanetPosition(i, 1, 500);
                vulcanoPos = calculatePlanetPosition(i, -5, 1000);
                betasoidePos = calculatePlanetPosition(i, 3, 2000);

                if (arePointsColineal(ferengiPos, vulcanoPos, betasoidePos, sunPosition))
                {
                    //Sequia
                    weather = "Sequia";
                } else if (arePointsColineal(ferengiPos, vulcanoPos, betasoidePos))
                {
                    //Optimo
                    weather = "Optimo";
                } else
                {
                    if (pointBelongsInTriangle(ferengiPos, vulcanoPos, betasoidePos, sunPosition))
                    {
                        //Lluvia
                        weather = "Lluvia";
                    } else
                    {
                        //Indefinido
                        weather = "Indefinido";
                    }
                }
                climates.day = i;
                climates.weather = weather;
                db.Climates.Add(climates); 
            }
            db.SaveChanges();

        }
        private Point calculatePlanetPosition(int day, int velocity, int sunDistance)
        {
            int degree = (day * velocity) % 360;
            degree = degree < 0 ? degree + 360 : degree;
            double x = Math.Cos(degree * Math.PI / 180) * sunDistance * 1000;
            double y = Math.Sin(degree * Math.PI / 180) * sunDistance * 1000;

            Point pos = new Point();

            pos.x = Math.Round(x, 2);
            pos.y = Math.Round(y, 2);

            return pos;
        }

        //Para ver si los puntos son colineales entre si, tengo que calcular la pendiente entre ambos, calculando las pendientes, las comparo y puedo verificar si pertenecen a una misma recta
        private bool arePointsColineal(Point a, Point b, Point c, Point d = null)
        {
            bool colineal = false;
            double pendAB = calculatePend(a, b);
            double pendAC = calculatePend(a, c);
            if (d != null)
            {
                double pendAD = calculatePend(a, d);
                if (pendAB == pendAC && pendAB == pendAD)
                {
                    colineal = true;
                }
            }
            else
            {
                if (pendAB == pendAC)
                {
                    colineal = true;
                }
            }
            return colineal;
        }

        private double calculatePend(Point a, Point b)
        {
            return (b.y - a.y) / (b.x - a.x);
        }

        //Para verificar que un punto este dentro de un triangulo, debo realizar el producto vectorial de los 3 puntos con el punto en cuestion, a traves del producto vectorial, si el signo de estos productos vectoriales es el mismo el punto se encuentra dentro del triangulo
        private bool pointBelongsInTriangle(Point a, Point b, Point c, Point point)
        {
            double VPab = vectorialProd(a, b, point);
            double VPcb = vectorialProd(b, c, point);
            double VPca = vectorialProd(c, a, point);

            return VPab > 0 && VPcb > 0 && VPca > 0 || VPab < 0 && VPcb < 0 && VPca < 0;
        }

        private double vectorialProd(Point a, Point b, Point c)
        {
            return (a.x - c.x) * (b.y - c.y) - (b.x - c.x) * (a.y - c.y);
        }


        
    }
}
