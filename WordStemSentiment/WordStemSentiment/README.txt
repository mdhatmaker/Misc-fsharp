WordStemSentiment (F# project)

This project attempts to combine word "stemming" and sentiment analysis to achieve
more accurate sentiment prediction with ML.
https://www.codesuji.com/2018/04/24/FSharp-and-Word-Stems/










-------------------------------------------------------------------------------
NOTES

Example rows from the Kaggle database:
// train.csv
"id","qid1","qid2","question1","question2","is_duplicate"                                                                
"0","1","2","What is the step by step guide to invest in share market in india?","What is the step by step guide to inves
t in share market?","0"                                                                                                  
"1","3","4","What is the story of Kohinoor (Koh-i-Noor) Diamond?","What would happen if the Indian government stole the K
ohinoor (Koh-i-Noor) diamond back?","0"                                                                                  

// test.csv
"test_id","question1","question2"
0,"How does the Surface Pro himself 4 compare with iPad Pro?","Why did Microsoft choose core m3 and not core i3 home Surface Pro 4?"
1,"Should I have a hair transplant at age 24? How much would it cost?","How much cost does hair transplant require?"

// submission.csv
test_id,is_duplicate
0,0.425764
1,0.212075







-------------------------------------------------------------------------------
MISC REFERENCES

https://www.codesuji.com/2018/01/06/Tackling-Kaggle-FSharp-XGBoost/

