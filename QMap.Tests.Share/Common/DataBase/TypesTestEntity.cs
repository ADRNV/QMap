namespace QMap.Tests.Share.DataBase
{
    public class TypesTestEntity
    {
        public int Id { get; set; }

        public int IntField { get; set; }

        public string StringField { get; set; }

        public DateTime DateTimeField { get; set; }

        public byte[] ByteField { get; set; }

        public bool BoolField { get; set; }

        public override bool Equals(object? obj)
        {
            var sameObject = obj as TypesTestEntity;

            if (sameObject is null)
            {
                return false;
            }

            return
                (sameObject.Id == this.Id
                && sameObject.IntField == this.IntField
                && sameObject.StringField == this.StringField
                && sameObject.DateTimeField == this.DateTimeField
                && sameObject.ByteField == this.ByteField
                && sameObject.BoolField == this.BoolField);
        }
    }
}
