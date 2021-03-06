﻿using SharpMap.Styles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BruTile;
using ProjNet;
using ProjNet.CoordinateSystems;
using System.IO;

namespace SharpMappingWinform
{

    public partial class Form1 : Form
    {
        public Dictionary<int, IStyle> styles = new Dictionary<int, IStyle>();
        public Form1()
        {
            InitializeComponent();

            string currentDir = Directory.GetCurrentDirectory();
            string tutFile = @"\ShapeFiles\states_ugl.shp";
            string worldBorders = @"\ShapeFiles\TM_WORLD_BORDERS-0.3.shp";
            List<string> texasShapeFiles = new List<string>();


            SharpMap.Layers.VectorLayer vlay = new SharpMap.Layers.VectorLayer("world");

            // Load information from ShapeFiles
            vlay.DataSource = new SharpMap.Data.Providers.ShapeFile(currentDir + worldBorders, true);

            //Create the style
            VectorStyle defaultStyle = new VectorStyle();

            //Create the style for regions
            VectorStyle africa = new VectorStyle();
            VectorStyle antartica = new VectorStyle();
            VectorStyle australia = new VectorStyle();
            VectorStyle northAmerica = new VectorStyle();
            VectorStyle asia = new VectorStyle();
            VectorStyle europe = new VectorStyle();

            // Set Random Colors
            Random rnd = new Random();


            // Add styles to List
            styles.Add(999, defaultStyle);
            styles.Add(0, antartica);
            styles.Add(2, africa);
            styles.Add(9, australia);
            styles.Add(19, northAmerica);
            styles.Add(142, asia);
            styles.Add(150, europe);

            foreach (KeyValuePair<int, IStyle> style in styles)
            {
                var o = style.Value as VectorStyle;
                o.EnableOutline = true;
                rnd.Next(0, 256);

                var r = rnd.Next(0, 256);
                var g = rnd.Next(0, 256);
                var b = rnd.Next(0, 256);
                o.Fill = new SolidBrush(Color.FromArgb(r, g, b));
            }

            //Assign the theme
            vlay.Theme = new SharpMap.Rendering.Thematics.UniqueValuesTheme<int>("region", styles, defaultStyle);

            // Add the layer to the mapBox control
            mapBox1.Map.Layers.Add(vlay);

            // Get US Cities from WMS Server
            SharpMap.Layers.WmsLayer wmsL =
               new SharpMap.Layers.WmsLayer("US Cities",
                   "http://sampleserver1.arcgisonline.com/ArcGIS/services/Specialty/ESRI_StatesCitiesRivers_USA/MapServer/WMSServer");

            //Force PNG format. Else we can't see through
            wmsL.SetImageFormat("image/png");
            //Force version 1.1.0
            wmsL.Version = "1.1.0";
            //Add layer named 2 in the service (Cities)
            wmsL.AddLayer("2");
            //Set the SRID
            wmsL.SRID = 4326;
            mapBox1.Map.Layers.Add(wmsL);

            // mapBox1.Map.Layers.Add(vlay);
            mapBox1.Map.ZoomToExtents();
            mapBox1.Refresh();
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
        }

        private void randomColorStyles()
        {
            Random rnd = new Random();
            foreach (KeyValuePair<int, IStyle> style in styles)
            {
                var o = style.Value as VectorStyle;
                o.EnableOutline = true;
                rnd.Next(0, 256);

                var r = rnd.Next(0, 256);
                var g = rnd.Next(0, 256);
                var b = rnd.Next(0, 256);
                o.Fill = new SolidBrush(Color.FromArgb(r, g, b));
            }
            mapBox1.Refresh();
        }

        private void RandomColorButton_Click(object sender, EventArgs e)
        {
            randomColorStyles();
        }



    }
    
}
