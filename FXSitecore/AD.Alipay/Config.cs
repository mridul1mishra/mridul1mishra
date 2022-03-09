using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AD.Alipay
{
    public class Config
    {
        //partner ID,It's a 16-bit string start with "2088".Login in https://globalprod.alipay.com/order/myOrder.htm to see your partner ID.
        //Below is a default sandbox account for your reference,pls apply your own sandbox account here:https://global.alipay.com/help/integration/23
        public static string partner = Sitecore.Configuration.Settings.GetSetting("Alipay_PartnerId");

        //merchant's private key,the guide to generate merchant's public key and private key,pls refer to:https://global.alipay.com/service/website/25
        public static string private_key = HttpRuntime.AppDomainAppPath.ToString() + Sitecore.Configuration.Settings.GetSetting("Alipay_MerchantPrivateKeyRelativePath");

        //Alipay's public key
        //Pls use the Alipay's public key(alipay_public_key.pem) of production environment instead if you are in production environment
        public static string alipay_public_key = HttpRuntime.AppDomainAppPath.ToString() + Sitecore.Configuration.Settings.GetSetting("Alipay_AlipayPublicKeyRelativePath");

        //Page for receiving asynchronous Notification. It should be accessable from outer net.No custom parameters like '?id=123' permitted.
        public static string notify_url1 = Sitecore.Configuration.Settings.GetSetting("Alipay_NotifyUrl1");
        public static string notify_url2 = Sitecore.Configuration.Settings.GetSetting("Alipay_NotifyUrl2");
        public static string notify_url3 = Sitecore.Configuration.Settings.GetSetting("Alipay_NotifyUrl3");

        //Page for synchronous notification.It should be accessable from outer net.No custom parameters like '?id=123' permitted.
        public static string return_url = Sitecore.Configuration.Settings.GetSetting("Alipay_ReturnUrl");

        public static string alipay_from_email = Sitecore.Configuration.Settings.GetSetting("Alipay_From_Email");

        //Sign type
        public static string sign_type = "RSA2";

        // input_charset   gbk and utf-8 are supported now.
        public static string input_charset = "utf-8";

        public static string log_path = Sitecore.Configuration.Settings.DataFolder + "logs\\";

        //Service name of the interface.No need to modify.
        public static string service = "create_direct_pay_by_user";

        // 应用ID,您的APPID
        public static string app_id = Sitecore.Configuration.Settings.GetSetting("Alipay_AppId");

        // 支付宝网关
        public static string gatewayUrl = Sitecore.Configuration.Settings.GetSetting("Alipay_Gateway");

        // 商户私钥，您的原始格式RSA私钥
        public static string private_key_string = "MIIEpAIBAAKCAQEApvqXaUCv63a3mh8S6Xsmfg1tEslpLvSuOhXcyjkm7p31sAG4vQFYTPNM6xzNmQK01NsMSWdw+7757srzdw54QKRoQX35oYAIJaIJa3iVqNFHj6AmoNOMui3jQAthsJ1b71TNXhc72F3vTx2ij7kyJ7StQc7gSon1Alw5bTLZ3EVuIc/9sPCW/fD0ntysc2ymwtJvsvLJpRvmg8IBGTufEFOwusAJNqXIJ0o4trktBUcg8Mx/BXkTjM+7YJgYOfiKdufTx6MUH2fYBch6Asj2LIFo5qLitdU5b6QI5K8QEqEaPy3y25soX4FxEbq1prA4ZhIEzz+BPT7mZN5tWumgswIDAQABAoIBACgg9R5gY2bNIxaw/RSLeha1F096hAK63twqwOMAlTCnWZIw97rEhfoIRqYCSA7LrbWx5uQFLgvyO5J9x8EHAzNVz5BdvA5p/Is+w9DGLAFkOjR0IpRoAtwDhyevFI0jZCLCWJCSONmoyYhT8OtlcH5GV5UHKLQBL5afn1V08RKUYWTfqqkcRREpSO31Fp75cJPczGfm2zLrAiEn83W7OiP3YO7Ay3Bw3aPQJgkXvw79vKMvyYyTyeEterc1D2PC4MIGY6DwJNfrggUHILTUpQsBi6+iY5CPqavrn/rsYmXX04n74ib6lYVFuYvCLzOlqT5stRTsUVfCg8WPqbqQyVECgYEA3XlN5jI9W3VKyQ9kb89NWwBbdbQfxLikizlQzaqrrOBr0cH1xTBweoPiuQ+jy6NBaaLwxXVgUL9HQDhoX8WXrr3e7j0RHOGqcpEsEbiKNL4j5t4tjzs+m3lph1VIM98mQ71xaTdiR9R/DJi+ce3nA6PaUy2KqWmxr7WrQwsYBkcCgYEAwQJ5D+hu7jTnJuySdh02nIVLglKC/9LYu7059byq7gG4Q8bcwnXFP0I903Adu4Pym36tpi6mEMYRFMdAWtnUzj/04JxrtR52I9AzHlXLxu497nXYpp5kY3RORcTnaFbP9Cg4EMukcBFgfVl9VoOfGAUvSEeKBpqh7cSRF52zDDUCgYAH/t8fFmU8rglbJOEdYECMroCeyTf0ZC4ckJ8oLacxAJEj63qx+cO1yJkg0T26llRAOg+zMT+e6qjp2p96BaoqNtSbiTza7BK5PCB5K05iBRNKvG6soxjpiAKVTjDjHoVFIMu9XrB/o15K2CDYABy89udk5VoCL7yoeUiLjRT69QKBgQC32n8vpB1nU4WNEnVBhfprhp2y+p5GyrYhv9LiPmIkbgb0qQ3JPx4xcAwsyPPJl5sWe8k2L+GIwOOsg1DOOgn4nfvJqbb/xJCM9Np83wVJ7c+YwzEpJmBCTJvy4en8/hUBYv5lIVb2WdcsEH7QVnjKDMZJd7wHJm1xbHk5ocLYsQKBgQC9Qs8dG3FdkcOWAKiRTUWvb8ywo/w1GB3cSfNwuObbFTu/M97GKltmmkQSo4ljggVV6Pt2GW9QCputgPZNEaSVUWws2wZTrko1u7oVHIsraY9NPzuHkipOlN6B42uoniJWGfEbFUCbLAlg5oSKzXneZ4SEYa+6RUB6tx3/4okwzg==";

        // 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
        public static string alipay_public_key_string = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEApvqXaUCv63a3mh8S6Xsmfg1tEslpLvSuOhXcyjkm7p31sAG4vQFYTPNM6xzNmQK01NsMSWdw+7757srzdw54QKRoQX35oYAIJaIJa3iVqNFHj6AmoNOMui3jQAthsJ1b71TNXhc72F3vTx2ij7kyJ7StQc7gSon1Alw5bTLZ3EVuIc/9sPCW/fD0ntysc2ymwtJvsvLJpRvmg8IBGTufEFOwusAJNqXIJ0o4trktBUcg8Mx/BXkTjM+7YJgYOfiKdufTx6MUH2fYBch6Asj2LIFo5qLitdU5b6QI5K8QEqEaPy3y25soX4FxEbq1prA4ZhIEzz+BPT7mZN5tWumgswIDAQAB";

        // 编码格式
        public static string charset = "UTF-8";

    }
}