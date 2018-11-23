using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using UnityEngine;

namespace FullbyteGames.Utilities
{
    /// <summary>
    /// Test cases for the UnityJsonConverter class
    /// </summary>
    public class UnityJsonConverterTests
    {
        private const float FLOAT_COMPARISON_DELTA = 0.00001f;

        [Test]
        public void Vector2Test()
        {
            Vector2 testValue = Random.insideUnitCircle;
            //Convert to string
            var asString = WriteToString(testValue);
            //Test the array values
            var asJsonArray = JArray.Parse(asString);
            for (int i = 0; i < 2; i++)
            {
                Assert.AreEqual(testValue[i], asJsonArray[i].ToObject<float>(), FLOAT_COMPARISON_DELTA);
            }

            //deserialize
            var deserializedvalue = ReadFromString<Vector2>(asString);

            //Test the resultant object, Unity overloads the == operator to do appropriate float comparisons for vectors
            Assert.IsTrue(testValue == deserializedvalue);
        }

        [Test]
        public void Vector3Test()
        {
            Vector3 testValue = Random.insideUnitSphere;
            //Convert to string
            var asString = WriteToString(testValue);
            //Test the array values
            var asJsonArray = JArray.Parse(asString);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(testValue[i], asJsonArray[i].ToObject<float>(), FLOAT_COMPARISON_DELTA);
            }

            //deserialize
            var deserializedvalue = ReadFromString<Vector3>(asString);

            //Test the resultant object, Unity overloads the == operator to do appropriate float comparisons for vectors
            Assert.IsTrue(testValue == deserializedvalue);
        }

        [Test]
        public void Vector4Test()
        {
            Vector4 testValue = Random.insideUnitSphere;
            testValue.w = Random.value;
            //Convert to string
            var asString = WriteToString(testValue);
            //Test the array values
            var asJsonArray = JArray.Parse(asString);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(testValue[i], asJsonArray[i].ToObject<float>(), FLOAT_COMPARISON_DELTA);
            }

            //deserialize
            var deserializedvalue = ReadFromString<Vector4>(asString);

            //Test the resultant object, Unity overloads the == operator to do appropriate float comparisons for vectors
            Assert.IsTrue(testValue == deserializedvalue);
        }

        [Test]
        public void QuaternionTest()
        {
            Quaternion testValue = Random.rotation;
            //Convert to string
            var asString = WriteToString(testValue);
            //Test the array values
            var asJsonArray = JArray.Parse(asString);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(testValue[i], asJsonArray[i].ToObject<float>(), FLOAT_COMPARISON_DELTA);
            }

            //deserialize
            var deserializedvalue = ReadFromString<Quaternion>(asString);

            //Test the resultant object, Unity overloads the == operator to do appropriate float comparisons for vectors
            Assert.IsTrue(testValue == deserializedvalue);
        }

        [Test]
        public void QuaternionWFirstTest()
        {
            //Create special serializer and configure the converter to put w values first
            var serializer = GetConfiguredSerializer(out var jsonConverter);
            jsonConverter.QuaternionWComponentFirst = true;

            Quaternion testValue = Random.rotation;
            //Convert to string
            var asString = WriteToString(testValue, serializer);
            //Test the array values
            var asJsonArray = JArray.Parse(asString);

            //Test for w first
            Assert.AreEqual(testValue[3], asJsonArray[0].ToObject<float>(), FLOAT_COMPARISON_DELTA);

            //Check that xyz are in positions 1,2 and 3
            for (int i = 0; i < 3; i++)
            {                                 //offset json array as w is in the first index
                Assert.AreEqual(testValue[i], asJsonArray[i+1].ToObject<float>(), FLOAT_COMPARISON_DELTA);
            }

            //deserialize
            var deserializedvalue = ReadFromString<Quaternion>(asString, serializer);

            //Test the resultant object, Unity overloads the == operator to do appropriate float comparisons for vectors
            Assert.IsTrue(testValue == deserializedvalue);
        }

        [Test]
        public void Matrix4x4Test()
        {
            Matrix4x4 testValue = new Matrix4x4();
            for (int i = 0; i < 16; i++)
            {
                testValue[i] = Random.value;
            }
            //Convert to string
            var asString = WriteToString(testValue);
            //Test the array values
            var asJsonArray = JArray.Parse(asString);
            for (int i = 0; i < 16; i++)
            {
                Assert.AreEqual(testValue[i], asJsonArray[i].ToObject<float>(), FLOAT_COMPARISON_DELTA);
            }

            //deserialize
            var deserializedvalue = ReadFromString<Matrix4x4>(asString);

            //Test the resultant object, Unity overloads the == operator to do appropriate float comparisons for vectors
            Assert.IsTrue(testValue == deserializedvalue);
        }

        [Test]
        public void RayTest()
        {
            Ray testValue = new Ray
            {
                origin = Random.insideUnitSphere,
                direction = Random.onUnitSphere
            };

            //Convert to string
            var asString = WriteToString(testValue);
            //Test the array values
            var asJsonArray = JArray.Parse(asString);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(testValue.origin[i], asJsonArray[i].ToObject<float>(), FLOAT_COMPARISON_DELTA);
            }

            for (int i = 0; i < 3; i++)
            {                                //Offset for direction
                Assert.AreEqual(testValue.direction[i], asJsonArray[i+3].ToObject<float>(), FLOAT_COMPARISON_DELTA);
            }

            //deserialize
            var deserializedvalue = ReadFromString<Ray>(asString);

            //Test the resultant object, Unity overloads the == operator to do appropriate float comparisons for vectors
            Assert.IsTrue(testValue.origin == deserializedvalue.origin);
            Assert.IsTrue(testValue.direction == deserializedvalue.direction);
        }

        [Test]
        public void PlaneTest()
        {
            Plane testValue = new Plane
            {
                normal = Random.onUnitSphere,
                distance = Random.value
            };

            //Convert to string
            var asString = WriteToString(testValue);
            //Test the array values
            var asJsonArray = JArray.Parse(asString);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(testValue.normal[i], asJsonArray[i].ToObject<float>(), FLOAT_COMPARISON_DELTA);
            }
            //Offset for distance
            Assert.AreEqual(testValue.distance, asJsonArray[3].ToObject<float>(), FLOAT_COMPARISON_DELTA);

            //deserialize
            var deserializedvalue = ReadFromString<Plane>(asString);

            //Test the resultant object, Unity overloads the == operator to do appropriate float comparisons for vectors
            Assert.IsTrue(testValue.normal == deserializedvalue.normal);
            Assert.AreEqual(testValue.distance , deserializedvalue.distance, FLOAT_COMPARISON_DELTA);
        }


        private string WriteToString(object obj, JsonSerializer serializer = null)
        {
            if (serializer == null)
            {
                serializer = GetConfiguredSerializer();
            }
            using (var sw = new StringWriter())
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, obj);
                }
               return sw.GetStringBuilder().ToString();
            }
        }

        private T ReadFromString<T>(string asString, JsonSerializer serializer = null)
        {
            if(serializer == null)
            {
                serializer = GetConfiguredSerializer();
            }

            using (var sr = new StringReader(asString))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize<T>(reader);
                }
            }
        }

        private JsonSerializer GetConfiguredSerializer()
        {
            var serializer = new JsonSerializer();
            var converter = new UnityJsonConverter();
            serializer.Converters.Add(converter);
            return serializer;
        }

        private JsonSerializer GetConfiguredSerializer(out UnityJsonConverter converter)
        {
            var serializer = new JsonSerializer();
            converter = new UnityJsonConverter();
            serializer.Converters.Add(converter);
            return serializer;
        }
    }
}