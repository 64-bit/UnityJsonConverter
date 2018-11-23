# Unity Json Converter

This project is a Json Value converter for use with Unity and the Newtonsoft Json.NET library.

By default, this library will not correctly serialize Unity math types (Vector3, Quaternion, ect...) as they have a number of public properties that will be serialized as well. The UnityJsonConverter provides a implementation for correctly serialzing and deserialzing the Unity math types.

Note: This value converter will not work for entire MonoBehaviours, only classes and structs that contain the Unity math types.

## Getting Started
### Prerequisites

This project requires the Newtonsoft Json.NET library (https://www.newtonsoft.com/json)
This project requires `.NET 4.x` API compatibility to be enabled in the Unity player settings

### Installing

Copy _```UnityJsonConverter.cs```_ into your Unity project Assets folder.
Optionally include _```/Editor/UnityJsonConverterTests.cs```_ if you would like to run the unit tests.

### Usage
Construct a _UnityJustonConverter_ and pass it to the _JsonConverter[]_ argument in _JsonConvert.SerializeObject()_ or _JsonConvert.DeserializeObject()_
````
            var vector = new Vector3(0.0f, 1.0f, 2.0f);
            var unityJsonConverter = new UnityJsonConverter();

            //Returns the string '[0.0,1.0,2.0]'
            var result = JsonConvert.SerializeObject(vector, unityJsonConverter);
````
Construct a _JsonSerializer_ and add the _UnityJsonConverter_ to the _Converters_ collection, then use the class as normal
````
var vector = new Vector3(0.0f,1.0f,2.0f);

var serializer = new JsonSerializer();
var unityJsonConverter = new UnityJsonConverter();
serializer.Converters.Add(unityJsonConverter);

using (var stringWriter = new StringWriter())
{
    using (JsonWriter writer = new JsonTextWriter(stringWriter))
    {
        serializer.Serialize(writer, vector);
    }

    //Returns the string '[0.0,1.0,2.0]'
    var result = stringWriter.ToString();
}
````

### Supported Types
The following types are currently supported by the converter.
````
UnityEngine.Vector2
UnityEngine.Vector3
UnityEngine.Vector4
UnityEngine.Quaternion
UnityEngine.Matrix4x4
UnityEngine.Ray
UnityEngine.Plane
````
All supported types are written to a JSON array of numbers
Vector values are written in xyzw der
Quaterion values are written in xyzw order by default
Quaterion values can be written in wxyz order by setting the _UnityJsonConverter.QuaternionWComponentFirst_ field to true
Matrix values are written in the order the [] accessor reads values
Ray values are written as ray.origin.xyz ray.direction.xyz
Plane values are written as plane.normal.xyz plane.distance

## Running the tests

The unit tests for this project are setup to run under the Unity test runner.
They will show up in the test runner under _FullbyteGames/Utilities/UnityJsonConverterTests_


## Authors

* **Jonathan Linsner** - *Initial work* - [64-bit on github](https://github.com/64-bit/)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Thanks to Newtonsoft for the great JSON parsing library
