using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace ServiciosEducativosComunitarios.Converters
{
    public class IdToNameConverter : IValueConverter
    {
        private static readonly Dictionary<int, string> MunicipioMap = new Dictionary<int, string>
        {
            {0, "Seleccionar..."},
            {1, "Apozol"},
            {2, "Apulco"},
            {3, "Atolinga"},
            {4, "Benito Juárez"},
            {5, "Calera"},
            {6, "Cañitas de Felipe Pescador"},
            {7, "Concepción del Oro"},
            {8, "Cuauhtémoc"},
            {9, "Chalchihuites"},
            {10, "Fresnillo"},
            {11, "Trinidad García de la Cadena"},
            {12, "Genaro Codina"},
            {13, "General Enrique Estrada"},
            {14, "General Francisco R. Murguía"},
            {15, "El Plateado de Joaquín Amaro"},
            {16, "General Pánfilo Natera"},
            {17, "Guadalupe"},
            {18, "Huanusco"},
            {19, "Jalpa"},
            {20, "Jerez"},
            {21, "Jiménez del Teul"},
            {22, "Juan Aldama"},
            {23, "Juchipila"},
            {24, "Loreto"},
            {25, "Luis Moya"},
            {26, "Mazapil"},
            {27, "Melchor Ocampo"},
            {28, "Mezquital del Oro"},
            {29, "Miguel Auza"},
            {30, "Momax"},
            {31, "Monte Escobedo"},
            {32, "Morelos"},
            {33, "Moyahua de Estrada"},
            {34, "Nochistlán de Mejía"},
            {35, "Noria de Ángeles"},
            {36, "Ojocaliente"},
            {37, "Pánuco"},
            {38, "Pinos"},
            {39, "Río Grande"},
            {40, "Sain Alto"},
            {41, "El Salvador"},
            {42, "Sombrerete"},
            {43, "Susticacán"},
            {44, "Tabasco"},
            {45, "Tepechitlán"},
            {46, "Tepetongo"},
            {47, "Teúl de González Ortega"},
            {48, "Tlaltenango de Sánchez Román"},
            {49, "Valparaíso"},
            {50, "Vetagrande"},
            {51, "Villa de Cos"},
            {52, "Villa García"},
            {53, "Villa González Ortega"},
            {54, "Villa Hidalgo"},
            {55, "Villanueva"},
            {56, "Zacatecas"},
            {57, "Trancoso"},
            {58, "Santa María de la Paz"}
        };

        private static readonly Dictionary<int, string> AmbitoMap = new Dictionary<int, string>
        {
            {0, "Seleccionar..."},
            {1, "Rural"},
            {2, "Urbano"}
        };

        private static readonly Dictionary<int, string> ProgramMap = new Dictionary<int, string>
        {
            {0, "Seleccionar..."},
            {1, "Preescolar"},
            {2, "Primaria"},
            {3, "Secundaria"}
        };

        private static readonly Dictionary<int, string> PeriodMap = new Dictionary<int, string>
        {
            {0, "Seleccionar..."},
            {2028, "2028-2029"},
            {2027, "2027-2028"},
            {2026, "2026-2027"},
            {2025, "2025-2026"},
            {2024, "2024-2025"},
            {2023, "2023-2024"},
            {2022, "2022-2023"}
        };


        private static readonly Dictionary<int, string> StatusMap = new Dictionary<int, string>
        {
            {0, "Seleccionar..."},
            {1, "Activo"},
            {2, "Baja"},
            {3, "Clausura"},
            {4, "Reapertura"}
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string param && !string.IsNullOrEmpty(param))
            {
                int id;

                if (value is int)
                {
                    id = (int)value;
                }
                else if (!int.TryParse(value?.ToString(), out id))
                {
                    return value?.ToString() ?? string.Empty;
                }

                if (param.Equals("Municipio", StringComparison.OrdinalIgnoreCase))
                {
                    if (MunicipioMap.TryGetValue(id, out var name))
                    {
                        return name;
                    }
                }
                else if (param.Equals("Ambito", StringComparison.OrdinalIgnoreCase))
                {
                    if (AmbitoMap.TryGetValue(id, out var name))
                    {
                        return name;
                    }
                }
                else if (param.Equals("Program", StringComparison.OrdinalIgnoreCase))
                {
                    if (ProgramMap.TryGetValue(id, out var name))
                    {
                        return name;
                    }
                }
                else if (param.Equals("Period", StringComparison.OrdinalIgnoreCase))
                {
                    if (PeriodMap.TryGetValue(id, out var name))
                    {
                        return name;
                    }
                }
                else if (param.Equals("Status", StringComparison.OrdinalIgnoreCase))
                {
                    if (StatusMap.TryGetValue(id, out var name))
                    {
                        return name;
                    }
                }
            }

            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Se usa sólo para mostrar; no hace falta convertir de vuelta en el DataGrid.
            throw new NotSupportedException();
        }
    }
}
