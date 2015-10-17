using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Utility;
using Model;

namespace HzHospitalRegister
{
	internal class RegisterHelper
	{
		private const string AREA_LIST_URL = "http://www.zj12580.cn/area";

		private const string HOS_LIST_URL = "http://www.zj12580.cn/hos/list/";

		private const string DEPT_LIST_URL = "http://www.zj12580.cn/dept/list/";

		private const string DOCTER_LIST_URL = "http://www.zj12580.cn/doc/list/";

		private const string DEPT_INFO_URL = "http://www.zj12580.cn/dept/queryDepartInfo/";

		private const string AUTHCODE_URL = "http://www.zj12580.cn/authCode.svl?type=captcha&time=";

		private const string ORDER_URL = "http://www.zj12580.cn/regCaptcha.svl?";

		private const string CHECK_CAP_CODE_URL = "http://www.zj12580.cn/order/capchk?";

		private const string LOGIN_URL = "http://www.zj12580.cn/login";

		private const string INDEX_URL = "http://www.zj12580.cn/";

		private const string LOGOUT_URL = "http://www.zj12580.cn/logout";

		private const string ORDER_SAVE_URL = "http://www.zj12580.cn/order/save";

		private const string ORDER_CHECK_URL = "http://www.zj12580.cn/order/check";

		private const string ROOT_XPATH = "/html/body/div[@id='middle']/div[@class='middle_content']/div[@class='right_boxs']/div[@class='right_box_1']/div[@class='right_box_1_r']/table";

		private const string LOGIN_RESULT_XPATH = "/html/body/div[@id='middle_login']/div[@class='right_box']/p[@class='center']/span";

		private const string NON_USRR_XPATH = "/html/body/div[@id='middle_pwd']";

		private const string USER_INFO_XPATH = "/html/body/div[@class='header_3']/div[@class='ad_box']/div[@class='ad']/div[@class='login_next_box']/div[@class='login_next']/table";

		private const string ORDER_INFO_XPATH = "/html/body/div[@id='middle']/div[@class='m_b']";

		private const string ORDER_SUCCESS_XPATH = "/html/body/div[@id='middle']/div[@class='m_b']/div[@class='succe']/p";

		private static RegisterHelper _instance;

		private HttpHelper _httpHelper;

		private HttpItem _httpItem;

		private HtmlDocument _htmlDoc;

		private UserInfo m_userInfo = new UserInfo();

		private OrderInfo m_orderInfo = new OrderInfo();

		private RegDoctor m_regDoctor = new RegDoctor();

		private RegDept m_regDept = new RegDept();

		private bool m_bIsLogin;

		public bool IsLogin
		{
			get
			{
				return this.m_bIsLogin;
			}
		}

		public static RegisterHelper Instance
		{
			get
			{
				if (RegisterHelper._instance == null)
				{
					RegisterHelper._instance = new RegisterHelper();
				}
				return RegisterHelper._instance;
			}
		}

		private RegisterHelper()
		{
			this._httpHelper = new HttpHelper();
			this._httpItem = new HttpItem();
			this.m_regDoctor.Tags = new string[16];
			this.m_regDoctor.ToolTipTexts = new string[16];
			this.m_regDoctor.Values = new string[16];
			this._htmlDoc = new HtmlDocument();
		}

		public IntegratedArea GetArea()
		{
			IntegratedArea result = null;
			try
			{
				this._httpItem.URL = "http://www.zj12580.cn/area";
				this._httpItem.ResultType = ResultType.String;
				this._httpItem.Method = "GET";
				this._httpItem.ContentType = "text/html";
				result = JsonConvert.DeserializeObject<IntegratedArea>(this._httpHelper.GetHtml(this._httpItem).Html);
			}
			catch (System.Exception err)
			{
				Log.WriteError("获取地区信息失败:", err);
			}
			return result;
		}

		public IntergetedHospital GetHispital(string id)
		{
			IntergetedHospital result = null;
			try
			{
				this._httpItem.URL = "http://www.zj12580.cn/hos/list/" + id;
				this._httpItem.ResultType = ResultType.String;
				this._httpItem.Method = "GET";
				this._httpItem.ContentType = "text/html";
				this._httpItem.Postdata = string.Empty;
				result = JsonConvert.DeserializeObject<IntergetedHospital>(this._httpHelper.GetHtml(this._httpItem).Html);
			}
			catch (System.Exception err)
			{
				Log.WriteError("获取医院信息失败", err);
			}
			return result;
		}

		public IntergretedDepartment GetDepartment(string id)
		{
			IntergretedDepartment result = null;
			try
			{
				this._httpItem.URL = "http://www.zj12580.cn/dept/list/" + id;
				this._httpItem.ResultType = ResultType.String;
				this._httpItem.Method = "GET";
				this._httpItem.ContentType = "text/html";
				this._httpItem.Postdata = string.Empty;
				result = JsonConvert.DeserializeObject<IntergretedDepartment>(this._httpHelper.GetHtml(this._httpItem).Html);
			}
			catch (System.Exception err)
			{
				Log.WriteError("获取科室信息失败", err);
			}
			return result;
		}

		public RegDept GetDepartmenInfo(string hosID, string deptName)
		{
			try
			{
				this.m_regDept.Reset();
				this.m_regDept.ResResult = ResponseReuslt.SUCCESS;
				this._httpItem.URL = "http://www.zj12580.cn/dept/queryDepartInfo/" + hosID + "/" + deptName;
				this._httpItem.ResultType = ResultType.String;
				this._httpItem.Method = "POST";
				this._httpItem.ContentType = "text/html";
				this._httpItem.Postdata = string.Empty;
				this._htmlDoc.LoadHtml(this._httpHelper.GetHtml(this._httpItem).Html);
				HtmlNode htmlNode = this._htmlDoc.DocumentNode;
				htmlNode = htmlNode.SelectSingleNode("/html/body/div[@id='middle']/div[@class='middle_content']/div[@class='right_boxs']/div[@class='right_box_1']/div[@class='right_box_1_r']/table");
				if (htmlNode == null)
				{
					this.m_regDept.ResResult = ResponseReuslt.ERROR_FAIL;
					RegDept regDept = this.m_regDept;
					return regDept;
				}
				HtmlNodeCollection htmlNodeCollection = htmlNode.SelectNodes("tr");
				if (htmlNodeCollection.Count == 2)
				{
					RegDept regDept = this.m_regDept;
					return regDept;
				}
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				foreach (HtmlNode current in ((System.Collections.Generic.IEnumerable<HtmlNode>)htmlNodeCollection))
				{
					string attributeValue = current.GetAttributeValue("class", "");
					if (attributeValue == string.Empty)
					{
						HtmlNodeCollection htmlNodeCollection2 = current.SelectNodes("td");
						if (htmlNodeCollection2.Count == 17)
						{
							int num = 0;
							foreach (HtmlNode current2 in ((System.Collections.Generic.IEnumerable<HtmlNode>)htmlNodeCollection2))
							{
								if (current2.SelectSingleNode("form") != null)
								{
									HtmlNode htmlNode2 = current2.SelectSingleNode("input[@type='submit']");
									if (htmlNode2 != null && num > 0)
									{
										this.m_regDoctor.Values[num - 1] = "<font face=\"Microsoft YaHei\" color=\"DarkGreen\">" + htmlNode2.GetAttributeValue("value", "").Replace("&#13;&#10;", "\r\n") + "</font>";
										this.m_regDoctor.ToolTipTexts[num - 1] = htmlNode2.GetAttributeValue("title", "");
										HtmlNodeCollection htmlNodeCollection3 = current2.SelectNodes("input");
										if (htmlNodeCollection3.Count == 14)
										{
											stringBuilder.Length = 0;
											stringBuilder.Append("http://www.zj12580.cn/order/num?");
											for (int i = 0; i < 13; i++)
											{
												stringBuilder.Append(htmlNodeCollection3[i].GetAttributeValue("name", ""));
												stringBuilder.Append("=");
												stringBuilder.Append(htmlNodeCollection3[i].GetAttributeValue("value", ""));
												if (i < 12)
												{
													stringBuilder.Append("&");
												}
											}
										}
										this.m_regDoctor.Tags[num - 1] = stringBuilder.ToString();
									}
									else
									{
										htmlNode2 = current2.SelectSingleNode("span");
										if (htmlNode2 != null)
										{
											string text = htmlNode2.InnerHtml.Trim();
											if (text == "已满")
											{
												this.m_regDoctor.Values[num - 1] = "<font face=\"Microsoft YaHei\" color=\"DarkRed\">" + text + "</font>";
												this.m_regDoctor.ToolTipTexts[num - 1] = text;
											}
											else if (text == "停诊")
											{
												this.m_regDoctor.Values[num - 1] = "<font face=\"Microsoft YaHei\" color=\"Blue\">" + text + "</font>";
												this.m_regDoctor.ToolTipTexts[num - 1] = text;
											}
											else if (text == "预约")
											{
												this.m_regDoctor.Values[num - 1] = "<font face=\"Microsoft YaHei\" color=\"Gray\">" + text + "</font>";
												if (num >= 15)
												{
													this.m_regDoctor.ToolTipTexts[num - 1] = "暂未放号";
												}
												else
												{
													this.m_regDoctor.ToolTipTexts[num - 1] = "已过预约";
												}
											}
											else
											{
												this.m_regDoctor.Values[num - 1] = "<font face=\"Microsoft YaHei\" color=\"Gray\">" + text + "</font>";
												this.m_regDoctor.ToolTipTexts[num - 1] = text;
											}
											this.m_regDoctor.Tags[num - 1] = null;
										}
									}
								}
								else if (num == 0)
								{
									if (current2.SelectSingleNode("p/a") == null)
									{
										this.m_regDoctor.Name = current2.InnerHtml.Trim();
									}
									else
									{
										this.m_regDoctor.Name = current2.SelectSingleNode("p/a").InnerHtml + "\n" + current2.SelectNodes("p")[1].InnerHtml;
									}
								}
								else
								{
									this.m_regDoctor.ToolTipTexts[num - 1] = null;
									this.m_regDoctor.Values[num - 1] = null;
									this.m_regDoctor.Tags[num - 1] = null;
								}
								num++;
							}
						}
						this.m_regDept.AddDoctor(this.m_regDoctor);
					}
					else if (attributeValue == "tr_t")
					{
						HtmlNodeCollection htmlNodeCollection4 = current.SelectNodes("td");
						if (htmlNodeCollection4.Count >= 9)
						{
							for (int j = 1; j < 9; j++)
							{
								this.m_regDept.Dates[j - 1] = htmlNodeCollection4[j].InnerHtml.Replace("<br>", "\n");
							}
						}
					}
				}
			}
			catch (System.Exception err)
			{
				Log.WriteError("科室信息获取失败", err);
				this.m_regDept.ResResult = ResponseReuslt.ERROR_UNKNOW;
			}
			return this.m_regDept;
		}

		public System.Drawing.Image GetAuthCode()
		{
			System.Drawing.Image result;
			try
			{
				System.Globalization.CultureInfo provider = new System.Globalization.CultureInfo("en-US");
				this._httpItem.URL = "http://www.zj12580.cn/authCode.svl?type=captcha&time=" + System.DateTime.Now.ToString("ddd MMM dd yyyy HH:mm:ss", provider) + " GMT 0800 (中国标准时间)";
				this._httpItem.ResultType = ResultType.Byte;
				this._httpItem.Method = "GET";
				this._httpItem.ContentType = "text/html";
				HttpResult html = this._httpHelper.GetHtml(this._httpItem);
				if (html.Cookie != null)
				{
					this._httpItem.Cookie = html.Cookie;
				}
				result = this.ConvertbyteArrayToImage(html.ResultByte);
			}
			catch (System.Exception err)
			{
				Log.WriteError("验证码获取失败", err);
				result = null;
			}
			return result;
		}

		private System.Drawing.Image ConvertbyteArrayToImage(byte[] Bytes)
		{
			System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(Bytes);
			System.Drawing.Image result = System.Drawing.Image.FromStream(memoryStream);
			memoryStream.Close();
			return result;
		}

		public string Login(string userName, string passwd, string authCode)
		{
			string result = string.Empty;
			try
			{
				this.m_bIsLogin = false;
				this._httpItem.URL = "http://www.zj12580.cn/login";
				this._httpItem.ResultType = ResultType.String;
				this._httpItem.Method = "POST";
				this._httpItem.Allowautoredirect = false;
				this._httpItem.Postdata = string.Format("username={0}&password={1}&captcha={2}", userName, passwd, authCode);
				this._httpItem.ContentType = "application/x-www-form-urlencoded";
				HttpResult html = this._httpHelper.GetHtml(this._httpItem);
				if (html.RedirectUrl == "http://www.zj12580.cn/")
				{
					this.m_bIsLogin = true;
					return string.Empty;
				}
				this._htmlDoc.LoadHtml(html.Html);
				HtmlNode htmlNode = this._htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[@id='middle_login']/div[@class='right_box']/p[@class='center']/span");
				if (htmlNode != null)
				{
					result = htmlNode.InnerHtml.Trim();
				}
				else if (this._htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[@id='middle_pwd']") != null)
				{
					result = "该用户名没有注册";
				}
				else
				{
					result = "登陆信息获取失败";
				}
			}
			catch (System.Exception ex)
			{
				Log.WriteError("登陆失败", ex);
				result = ex.Message;
			}
			return result;
		}

		public void Logout()
		{
			this._httpItem.URL = "http://www.zj12580.cn/logout";
			this._httpItem.ResultType = ResultType.String;
			this._httpItem.Method = "GET";
			this._httpItem.Postdata = string.Empty;
			this._httpItem.ContentType = "text/html";
			this._httpHelper.GetHtml(this._httpItem);
			this.m_bIsLogin = false;
			this._httpItem.Cookie = null;
		}

		public UserInfo GetUserInfo()
		{
			this.m_userInfo.Clear();
			try
			{
				this._httpItem.URL = "http://www.zj12580.cn/";
				this._httpItem.ResultType = ResultType.String;
				this._httpItem.Method = "GET";
				this._httpItem.Postdata = string.Empty;
				this._httpItem.ContentType = "text/html";
				this._httpItem.Allowautoredirect = true;
				HttpResult html = this._httpHelper.GetHtml(this._httpItem);
				this._htmlDoc.LoadHtml(html.Html);
				HtmlNode htmlNode = this._htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[@class='header_3']/div[@class='ad_box']/div[@class='ad']/div[@class='login_next_box']/div[@class='login_next']/table");
				if (htmlNode != null)
				{
					HtmlNodeCollection htmlNodeCollection = htmlNode.SelectNodes("tr");
					if (htmlNodeCollection.Count == 4)
					{
						this.m_userInfo.Name = htmlNodeCollection[0].ChildNodes[3].InnerHtml.Trim();
						this.m_userInfo.CardId = htmlNodeCollection[1].ChildNodes[3].InnerHtml.Trim();
						this.m_userInfo.PhoneNumber = htmlNodeCollection[2].ChildNodes[3].InnerHtml.Trim();
						this.m_userInfo.Credibility = htmlNodeCollection[3].ChildNodes[3].InnerHtml.Trim();
					}
				}
			}
			catch (System.Exception err)
			{
				Log.WriteError("用户信息获取失败", err);
			}
			return this.m_userInfo;
		}

		public OrderInfo GetQueryRegTime(string query)
		{
			this.m_orderInfo.Clear();
			try
			{
				this._httpItem.URL = query;
				this._httpItem.ResultType = ResultType.String;
				this._httpItem.Method = "GET";
				this._httpItem.Postdata = string.Empty;
				this._httpItem.ContentType = "text/html";
				this._httpItem.Allowautoredirect = false;
				HttpResult html = this._httpHelper.GetHtml(this._httpItem);
				if (html.RedirectUrl.Length == 0)
				{
					this._htmlDoc.LoadHtml(html.Html);
					HtmlNode htmlNode = this._htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[@id='middle']/div[@class='m_b']");
					if (htmlNode != null)
					{
						this.m_orderInfo.User = this.m_userInfo;
						this.m_orderInfo.ResResult = ResponseReuslt.SUCCESS;
						HtmlNodeCollection htmlNodeCollection = htmlNode.SelectNodes("div[@class='m_l']/div[@class='m_l_1']/div[@class='con_r']/p");
						if (htmlNodeCollection != null && htmlNodeCollection.Count == 3)
						{
							this.m_orderInfo.Doctor.Name = htmlNodeCollection[0].InnerHtml.Trim();
							this.m_orderInfo.Doctor.HospitalName = htmlNodeCollection[2].InnerHtml.Trim();
						}
						HtmlNode htmlNode2 = htmlNode.SelectSingleNode("input[@name='hosId']");
						if (htmlNode2 != null)
						{
							this.m_orderInfo.Doctor.HospitalId = htmlNode2.GetAttributeValue("value", "");
						}
						HtmlNodeCollection htmlNodeCollection2 = htmlNode.SelectNodes("input");
						if (htmlNodeCollection2 != null && htmlNodeCollection2.Count == 14)
						{
							System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
							stringBuilder.Append("numId={0}&resTime={1}&resNumber={2}&");
							for (int i = 3; i < 14; i++)
							{
								stringBuilder.Append(htmlNodeCollection2[i].GetAttributeValue("name", ""));
								stringBuilder.Append("=");
								stringBuilder.Append(htmlNodeCollection2[i].GetAttributeValue("value", ""));
								stringBuilder.Append("&");
							}
							stringBuilder.Append("num={3}");
							this.m_orderInfo.CheckOrderPost = stringBuilder.ToString();
						}
						HtmlNodeCollection htmlNodeCollection3 = htmlNode.SelectNodes("div[@class='m_l']/div[@class='m_l_2']/p/span");
						if (htmlNodeCollection3 != null && htmlNodeCollection3.Count == 3)
						{
							this.m_orderInfo.Fee = htmlNodeCollection3[0].InnerHtml.Trim();
							this.m_orderInfo.Doctor.Department = htmlNodeCollection3[1].InnerHtml.Trim();
							this.m_orderInfo.VisitDate = htmlNodeCollection3[2].InnerHtml.Trim().Replace("&nbsp;", " ");
						}
						HtmlNodeCollection htmlNodeCollection4 = htmlNode.SelectNodes("div[@class='m_r']/div[@class='m_r_1']/table/tr/td/input");
						if (htmlNodeCollection4 == null)
						{
							goto IL_360;
						}
						using (System.Collections.Generic.IEnumerator<HtmlNode> enumerator = ((System.Collections.Generic.IEnumerable<HtmlNode>)htmlNodeCollection4).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								HtmlNode current = enumerator.Current;
								string[] array = current.GetAttributeValue("value", "").Split(new char[]
								{
									','
								});
								if (array != null && array.Length == 3)
								{
									VisitTime visitTime = new VisitTime();
									visitTime.Index = array[2];
									visitTime.NumId = array[0];
									visitTime.Time = array[1];
									this.m_orderInfo.VisitTimes.Add(visitTime);
								}
							}
							goto IL_360;
						}
					}
					this.m_orderInfo.ResResult = ResponseReuslt.ERROR_PARSE;
				}
				else
				{
					this.m_orderInfo.ResResult = ResponseReuslt.NON_LOGIN;
				}
				IL_360:;
			}
			catch (System.Exception err)
			{
				Log.WriteError("获取预约订单信息失败", err);
				this.m_orderInfo.ResResult = ResponseReuslt.ERROR_UNKNOW;
			}
			return this.m_orderInfo;
		}

		public System.Drawing.Image GetOrderCode(string hospitalId, string numId)
		{
			System.Drawing.Image result;
			try
			{
				System.Globalization.CultureInfo provider = new System.Globalization.CultureInfo("en-US");
				this._httpItem.URL = "http://www.zj12580.cn/regCaptcha.svl?" + string.Format("hospitalId={0}&numId={1}&time={2}", hospitalId, numId, System.DateTime.Now.ToString("ddd MMM dd yyyy HH:mm:ss", provider) + " GMT 0800 (中国标准时间)");
				this._httpItem.ResultType = ResultType.Byte;
				this._httpItem.Method = "GET";
				this._httpItem.ContentType = "text/html";
				HttpResult html = this._httpHelper.GetHtml(this._httpItem);
				if (html.Cookie != null)
				{
					this._httpItem.Cookie = html.Cookie;
				}
				result = this.ConvertbyteArrayToImage(html.ResultByte);
			}
			catch (System.Exception err)
			{
				Log.WriteError("验证码获取失败", err);
				result = null;
			}
			return result;
		}

		public ResponseReuslt CheckOrderCode(string hospitalId, string numId, string capCode)
		{
			ResponseReuslt result = ResponseReuslt.SUCCESS;
			try
			{
				System.Globalization.CultureInfo provider = new System.Globalization.CultureInfo("en-US");
				this._httpItem.URL = "http://www.zj12580.cn/order/capchk?" + string.Format("hospitalId={0}&numId={1}&cap={2}&time={3}", new object[]
				{
					hospitalId,
					numId,
					capCode,
					System.DateTime.Now.ToString("ddd MMM dd yyyy HH:mm:ss", provider) + " GMT 0800 (中国标准时间)"
				});
				this._httpItem.ResultType = ResultType.String;
				this._httpItem.Method = "GET";
				this._httpItem.ContentType = "text/html";
				HttpResult html = this._httpHelper.GetHtml(this._httpItem);
				if (html.Html != "success")
				{
					result = ResponseReuslt.ERROR_FAIL;
				}
			}
			catch (System.Exception err)
			{
				result = ResponseReuslt.ERROR_UNKNOW;
				Log.WriteError("验证码验证失败", err);
			}
			return result;
		}

		public string CheckOrder(OrderInfo orderInfo, VisitTime visit)
		{
			string result = string.Empty;
			try
			{
				this._httpItem.URL = "http://www.zj12580.cn/order/check";
				this._httpItem.ResultType = ResultType.String;
				this._httpItem.Method = "POST";
				this._httpItem.Allowautoredirect = false;
				this._httpItem.PostEncoding = System.Text.Encoding.UTF8;
				this._httpItem.Postdata = string.Format(this.m_orderInfo.CheckOrderPost, new object[]
				{
					visit.NumId,
					visit.Time,
					visit.Index,
					string.Concat(new string[]
					{
						visit.NumId,
						",",
						visit.Time,
						",",
						visit.Index
					})
				});
				this._httpItem.ContentType = "application/x-www-form-urlencoded";
				HttpResult html = this._httpHelper.GetHtml(this._httpItem);
				if (html.StatusCode == System.Net.HttpStatusCode.OK)
				{
					this._htmlDoc.LoadHtml(html.Html);
					HtmlNode htmlNode = this._htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[@id='middle']/div[@class='m_b']");
					if (htmlNode != null)
					{
						HtmlNodeCollection htmlNodeCollection = htmlNode.SelectNodes("div[@class='m_r']/input");
						if (htmlNodeCollection != null && htmlNodeCollection.Count == 15)
						{
							System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
							for (int i = 0; i < 15; i++)
							{
								stringBuilder.Append(htmlNodeCollection[i].GetAttributeValue("name", ""));
								stringBuilder.Append("=");
								stringBuilder.Append(htmlNodeCollection[i].GetAttributeValue("value", ""));
								stringBuilder.Append("&");
							}
							result = stringBuilder.ToString();
						}
					}
				}
			}
			catch (System.Exception err)
			{
				Log.WriteError("订单确认失败", err);
			}
			return result;
		}

		public OrderSuccessInfo OrderSave(string post)
		{
			OrderSuccessInfo orderSuccessInfo = new OrderSuccessInfo();
			try
			{
				this._httpItem.URL = "http://www.zj12580.cn/order/save";
				this._httpItem.ResultType = ResultType.String;
				this._httpItem.Method = "POST";
				this._httpItem.Allowautoredirect = false;
				this._httpItem.Postdata = post;
				this._httpItem.PostEncoding = System.Text.Encoding.UTF8;
				this._httpItem.Encoding = System.Text.Encoding.UTF8;
				this._httpItem.ContentType = "application/x-www-form-urlencoded";
				HttpResult html = this._httpHelper.GetHtml(this._httpItem);
				if (html.StatusCode == System.Net.HttpStatusCode.OK)
				{
					this._htmlDoc.LoadHtml(html.Html);
					HtmlNode htmlNode = this._htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[@id='middle']/div[@class='m_b']/div[@class='m_r']/div[@class='succe']");
					if (htmlNode != null)
					{
						orderSuccessInfo.ResResult = ResponseReuslt.SUCCESS;
						HtmlNodeCollection htmlNodeCollection = htmlNode.SelectNodes("p/span");
						orderSuccessInfo.Passwd = htmlNodeCollection[0].InnerHtml;
						orderSuccessInfo.Phone = htmlNodeCollection[1].InnerHtml;
						orderSuccessInfo.ResTime = htmlNodeCollection[3].InnerHtml;
						orderSuccessInfo.ResNum = htmlNodeCollection[5].InnerHtml;
					}
					else
					{
						orderSuccessInfo.ResResult = ResponseReuslt.ERROR_FAIL;
					}
				}
				else
				{
					orderSuccessInfo.ResResult = ResponseReuslt.NON_NET;
				}
			}
			catch (System.Exception err)
			{
				orderSuccessInfo.ResResult = ResponseReuslt.ERROR_UNKNOW;
				Log.WriteError("订单预约失败", err);
			}
			return orderSuccessInfo;
		}
	}
}
