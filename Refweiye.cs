
namespace Demo.BarCodeProAPI
{
    using System.Runtime.Serialization;
    using System;


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "DocInfoModel", Namespace = "http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class DocInfoModel : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BarCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LineNOField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProcessCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CreatedByField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EquipNOField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string vfree1Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string vfree2Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string vfree3Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string vfree4Field;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string vfree5Field;

        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string BarCode
        {
            get
            {
                return this.BarCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BarCodeField, value) != true))
                {
                    this.BarCodeField = value;
                    this.RaisePropertyChanged("BarCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string LineNO
        {
            get
            {
                return this.LineNOField;
            }
            set
            {
                if ((object.ReferenceEquals(this.LineNOField, value) != true))
                {
                    this.LineNOField = value;
                    this.RaisePropertyChanged("LineNO");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string ProcessCode
        {
            get
            {
                return this.ProcessCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ProcessCodeField, value) != true))
                {
                    this.ProcessCodeField = value;
                    this.RaisePropertyChanged("ProcessCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 3)]
        public string CreatedBy
        {
            get
            {
                return this.CreatedByField;
            }
            set
            {
                if ((object.ReferenceEquals(this.CreatedByField, value) != true))
                {
                    this.CreatedByField = value;
                    this.RaisePropertyChanged("CreatedBy");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 4)]
        public string EquipNO
        {
            get
            {
                return this.EquipNOField;
            }
            set
            {
                if ((object.ReferenceEquals(this.EquipNOField, value) != true))
                {
                    this.EquipNOField = value;
                    this.RaisePropertyChanged("EquipNO");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 5)]
        public string vfree1
        {
            get
            {
                return this.vfree1Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.vfree1Field, value) != true))
                {
                    this.vfree1Field = value;
                    this.RaisePropertyChanged("vfree1");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 6)]
        public string vfree2
        {
            get
            {
                return this.vfree2Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.vfree2Field, value) != true))
                {
                    this.vfree2Field = value;
                    this.RaisePropertyChanged("vfree2");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 7)]
        public string vfree3
        {
            get
            {
                return this.vfree3Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.vfree3Field, value) != true))
                {
                    this.vfree3Field = value;
                    this.RaisePropertyChanged("vfree3");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 8)]
        public string vfree4
        {
            get
            {
                return this.vfree4Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.vfree4Field, value) != true))
                {
                    this.vfree4Field = value;
                    this.RaisePropertyChanged("vfree4");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 9)]
        public string vfree5
        {
            get
            {
                return this.vfree5Field;
            }
            set
            {
                if ((object.ReferenceEquals(this.vfree5Field, value) != true))
                {
                    this.vfree5Field = value;
                    this.RaisePropertyChanged("vfree5");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "DocReturnModel", Namespace = "http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class DocReturnModel : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MsgField;

        private bool IsSuccessField;

        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string Msg
        {
            get
            {
                return this.MsgField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MsgField, value) != true))
                {
                    this.MsgField = value;
                    this.RaisePropertyChanged("Msg");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
        public bool IsSuccess
        {
            get
            {
                return this.IsSuccessField;
            }
            set
            {
                if ((this.IsSuccessField.Equals(value) != true))
                {
                    this.IsSuccessField = value;
                    this.RaisePropertyChanged("IsSuccess");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "CheckInfoModel", Namespace = "http://tempuri.org/")]
    [System.SerializableAttribute()]
    public partial class CheckInfoModel : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BarCodeField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MOField;

        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string BarCode
        {
            get
            {
                return this.BarCodeField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BarCodeField, value) != true))
                {
                    this.BarCodeField = value;
                    this.RaisePropertyChanged("BarCode");
                }
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false)]
        public string MO
        {
            get
            {
                return this.MOField;
            }
            set
            {
                if ((object.ReferenceEquals(this.MOField, value) != true))
                {
                    this.MOField = value;
                    this.RaisePropertyChanged("MO");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "BarCodeProAPI.BarCodeProAPISoap")]
    public interface BarCodeProAPISoap
    {

        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 docInfoModels 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/Do", ReplyAction = "*")]
        Demo.BarCodeProAPI.DoResponse Do(Demo.BarCodeProAPI.DoRequest request);

        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 checkInfoModels 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/CheckMO", ReplyAction = "*")]
        Demo.BarCodeProAPI.CheckMOResponse CheckMO(Demo.BarCodeProAPI.CheckMORequest request);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class DoRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "Do", Namespace = "http://tempuri.org/", Order = 0)]
        public Demo.BarCodeProAPI.DoRequestBody Body;

        public DoRequest()
        {
        }

        public DoRequest(Demo.BarCodeProAPI.DoRequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class DoRequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public Demo.BarCodeProAPI.DocInfoModel[] docInfoModels;

        public DoRequestBody()
        {
        }

        public DoRequestBody(Demo.BarCodeProAPI.DocInfoModel[] docInfoModels)
        {
            this.docInfoModels = docInfoModels;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class DoResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "DoResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public Demo.BarCodeProAPI.DoResponseBody Body;

        public DoResponse()
        {
        }

        public DoResponse(Demo.BarCodeProAPI.DoResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class DoResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public Demo.BarCodeProAPI.DocReturnModel DoResult;

        public DoResponseBody()
        {
        }

        public DoResponseBody(Demo.BarCodeProAPI.DocReturnModel DoResult)
        {
            this.DoResult = DoResult;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CheckMORequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CheckMO", Namespace = "http://tempuri.org/", Order = 0)]
        public Demo.BarCodeProAPI.CheckMORequestBody Body;

        public CheckMORequest()
        {
        }

        public CheckMORequest(Demo.BarCodeProAPI.CheckMORequestBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CheckMORequestBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public Demo.BarCodeProAPI.CheckInfoModel[] checkInfoModels;

        public CheckMORequestBody()
        {
        }

        public CheckMORequestBody(Demo.BarCodeProAPI.CheckInfoModel[] checkInfoModels)
        {
            this.checkInfoModels = checkInfoModels;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped = false)]
    public partial class CheckMOResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Name = "CheckMOResponse", Namespace = "http://tempuri.org/", Order = 0)]
        public Demo.BarCodeProAPI.CheckMOResponseBody Body;

        public CheckMOResponse()
        {
        }

        public CheckMOResponse(Demo.BarCodeProAPI.CheckMOResponseBody Body)
        {
            this.Body = Body;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://tempuri.org/")]
    public partial class CheckMOResponseBody
    {

        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue = false, Order = 0)]
        public Demo.BarCodeProAPI.DocReturnModel CheckMOResult;

        public CheckMOResponseBody()
        {
        }

        public CheckMOResponseBody(Demo.BarCodeProAPI.DocReturnModel CheckMOResult)
        {
            this.CheckMOResult = CheckMOResult;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface BarCodeProAPISoapChannel : Demo.BarCodeProAPI.BarCodeProAPISoap, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BarCodeProAPISoapClient : System.ServiceModel.ClientBase<Demo.BarCodeProAPI.BarCodeProAPISoap>, Demo.BarCodeProAPI.BarCodeProAPISoap
    {

        public BarCodeProAPISoapClient()
        {
        }

        public BarCodeProAPISoapClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public BarCodeProAPISoapClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public BarCodeProAPISoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public BarCodeProAPISoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Demo.BarCodeProAPI.DoResponse Demo.BarCodeProAPI.BarCodeProAPISoap.Do(Demo.BarCodeProAPI.DoRequest request)
        {
            return base.Channel.Do(request);
        }

        public Demo.BarCodeProAPI.DocReturnModel Do(Demo.BarCodeProAPI.DocInfoModel[] docInfoModels)
        {
            Demo.BarCodeProAPI.DoRequest inValue = new Demo.BarCodeProAPI.DoRequest();
            inValue.Body = new Demo.BarCodeProAPI.DoRequestBody();
            inValue.Body.docInfoModels = docInfoModels;
            Demo.BarCodeProAPI.DoResponse retVal = ((Demo.BarCodeProAPI.BarCodeProAPISoap)(this)).Do(inValue);
            return retVal.Body.DoResult;
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Demo.BarCodeProAPI.CheckMOResponse Demo.BarCodeProAPI.BarCodeProAPISoap.CheckMO(Demo.BarCodeProAPI.CheckMORequest request)
        {
            return base.Channel.CheckMO(request);
        }

        public Demo.BarCodeProAPI.DocReturnModel CheckMO(Demo.BarCodeProAPI.CheckInfoModel[] checkInfoModels)
        {
            Demo.BarCodeProAPI.CheckMORequest inValue = new Demo.BarCodeProAPI.CheckMORequest();
            inValue.Body = new Demo.BarCodeProAPI.CheckMORequestBody();
            inValue.Body.checkInfoModels = checkInfoModels;
            Demo.BarCodeProAPI.CheckMOResponse retVal = ((Demo.BarCodeProAPI.BarCodeProAPISoap)(this)).CheckMO(inValue);
            return retVal.Body.CheckMOResult;
        }
    }
}
