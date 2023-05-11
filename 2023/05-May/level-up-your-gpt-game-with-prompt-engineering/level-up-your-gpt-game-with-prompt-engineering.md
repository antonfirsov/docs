---
post_title: Level up your GPT game with prompt engineering
author1: luquinta@microsoft.com
post_slug: gpt-prompt-engineering-openai-azure-dotnet
username: luquinta@microsoft.com
microsoft_alias: luquinta
featured_image: open-ai-dotnet-hero.png
categories: .NET, .NET Core, AI Machine Learning, Azure, Machine Learning
tags: OpenAI, AI, MachineLearning
summary: Learn what prompt engineering is and how to use it to improve the quality of your GPT completions. 
desired_publication_date: 2023-05-12
post_date: 2023-05-12 09:00:00
---

Welcome back to this blog series on OpenAI and .NET!

If you’re new here, check out our [first post](https://devblogs.microsoft.com/dotnet/getting-started-azure-openai-dotnet/) where we introduce the series and show you how to get started using OpenAI in .NET.

The focus of this post is on prompt engineering and how you can refine the inputs you provide to OpenAI models to produce more relevant responses. Let’s get started!

## What is a prompt?

A prompt is the user input provided to a model to generate completions. The prompt is what guides the model to generate responses known as completions. 

For more details on prompts and completions, see the [Get Started with OpenAI Completions in .NET](https://devblogs.microsoft.com/dotnet/get-started-with-open-ai-completions-with-dotnet/) article.

### The structure of a prompt

At minimum, a prompt consists of two components:

- Context
- Task / Query

Given the following prompt:

> Summarize this for a second-grade student:
> 
> Jupiter is the fifth planet from the Sun and the largest in the Solar System. It is a gas giant with a mass one-thousandth that of the Sun, but two-and-a-half times that of all the other planets in the Solar System combined. Jupiter is one of the brightest objects visible to the naked eye in the night sky, and has been known to ancient civilizations since before recorded history. It is named after the Roman god Jupiter.[19] When viewed from Earth, Jupiter can be bright enough for its reflected light to cast visible shadows,[20] and is on average the third-brightest natural object in the night sky after the Moon and Venus.

It can be broken down into:

- **Context:** Jupiter is the fifth planet from the Sun and the largest in the Solar System. It is a gas giant with a mass one-thousandth that of the Sun, but two-and-a-half times that of all the other planets in the Solar System combined. Jupiter is one of the brightest objects visible to the naked eye in the night sky, and has been known to ancient civilizations since before recorded history. It is named after the Roman god Jupiter.[19] When viewed from Earth, Jupiter can be bright enough for its reflected light to cast visible shadows,[20] and is on average the third-brightest natural object in the night sky after the Moon and Venus.
- **Task/Query:** Summarize this for a second-grade student:

## What is prompt engineering?

Prompt engineering is the process and techniques for composing prompts to produce output that more closely resembles your desired intent.

## Tips for composing prompts

While not an exhaustive list, the following are quick tips for improving the quality of your prompts and completions:

- Be clear and specific
- Provide sample outputs
- Provide relevant context
- Refine, refine, refine

### Be clear and specific

When crafting a prompt, the less details you provide, the more assumptions the model needs to make. Place boundaries and constraints in your prompt to guide the model to output the results you want.

For example, let's say you want to classify the sentiment of a social media post using the following prompt:

> Classify this post
>
> "My cat is adorable ❤️❤️"

You might get a response that looks like the following: `This post would be classified as a statement or opinion.`

As you can see, the post has been assigned an arbitrary category. However, that category has no relation to sentiment. By providing more guidance and constraints in the prompt, you can guide the model to produce the output you want.

When you update the prompt to be more precise, you're informing the model you want the output to represent the sentiment of the post and choose between the three categories of positive, neutral, or negative.

> Classify the sentiment of this post as positive, neutral, or negative
> 
> "My cat is adorable ❤️❤️"

You should get a result of `Postive` in this case which more closely resembles your original intent of classifying sentiment.

### Provide sample outputs

The quickest way to start generating outputs is to use the model's preconfigured settings it was trained on. This is known as zero-shot learning. By providing examples, preferably using similar data to the one you'll be working with, you can better guide the model to produce better outputs. This technique is known as few-shot learning.

For example, let's say you want to extract information from a document like an e-mail and generate a JSON object.

> Extract the cities and airport codes from this text as JSON:
> 
> "I want to fly from Los Angeles to Miami."

You might get a response like the following: 

```json
{
  "From": {
    "City": "Los Angeles",
    "Airport Code": "LAX"
  },
  "To": {
    "City": "Miami",
    "Airport Code": "MIA"
  }
}
```

Although that's right, the schema of the JSON object that's generated does not match the schema expected by my application. In that case, I can provide a sample of the output I expect to guide the model to format my output correctly. Given the following prompt:

> Extract the cities and airport codes from this text as JSON:
> 
> Text: "I want to fly from Los Angeles to Miami."
> JSON Output: 
> {
>   "Origin": {
>     "CityName": "Los Angeles",
>     "AirportCode": "LAX"
>   },
>   "Destination": {
>     "CityName": "Miami",
>     "AirportCode": "MIA"
>   }
> }
> 
> Text: "I want to fly from Orlando to Boston"
> JSON Output:

You can expect the model to produce output similar to the following:

```json
{
  "Origin": {
    "CityName": "Orlando",
    "AirportCode": "MCO"
  },
  "Destination": {
    "CityName": "Boston",
    "AirportCode": "BOS"
  }
}
```

### Provide relevant context

Models like GPT were trained on millions of documents and artifacts from all over the internet. Therefore, when you ask it to perform tasks like answering questions and you don't limit the scope of resources it can use to generate a response, it's likely that in the best case, you will get a feasible answer (though maybe wrong) and in the worst case, the answer will be fabricated. 

For example if you asked someone to write a summary of Harry Potter, you may be referring to several things: movies, books, or video games. While the characters and some elements may be similar across these mediums, the story lines may differ and as a result, you might get different answers that are plausible, but not correct.  

Similar to zero-shot and few-shot learning which provides examples of the outputs you expect the model to generate, you can provide facts and additional relevant information in your prompt to guide the model to answer questions and perform various other tasks. This technique is known as grounding because you're grounding the model on facts. At a very high-level, this is how some of the AI capabilities in Bing work. A search is first performed to find the most relevant documents to answer your query. The contents from the most relevant web pages are then provided as additional context in your prompt and the AI models use this information to generate a more relevant response.  

Let's say you wanted to answer some questions about a document. This document can be a public webpage like Wikipedia or a document from your company's internal knowledge-base. Your prompt which includes additional information in the context might look like the following:

> Jupiter is the fifth planet from the Sun and the largest in the Solar System. It is a gas giant with a mass one-thousandth that of the Sun, but two-and-a-half times that of all the other planets in the Solar System combined. Jupiter is one of the brightest objects visible to the naked eye in the night sky, and has been known to ancient civilizations since before recorded history. It is named after the Roman god Jupiter.[19] When viewed from Earth, Jupiter can be bright enough for its reflected light to cast visible shadows,[20] and is on average the third-brightest natural object in the night sky after the Moon and Venus.
> 
> Answer the following question:
> 
> Q: Which is the fifth planet from the sun?
> A: Jupiter
> 
> Q: What's the mass of Jupiter compared to the sun?

You might get a response similar to the following: `A: Jupiter has a mass one-thousandth that of the Sun.`

In this example, not only do you provide facts and information for the model to use as part of its response, but you also provided an example of how you wanted it to respond.

### Refine, refine, refine

Generating outputs can be a process of trial and error. Don't be discouraged if you don't get the output you expect on the first try. Experiment with one or more of the techniques from this article and linked resources to find what works best for your use case. Reuse the initial set of outputs generated by the model to provide additional context and guidance in your prompt.

## Get started engineering your own prompts

Now that you know a few ways to improve your prompts and completions, it's time to get started generating your own. To get started: 

1. Sign up or request access with [OpenAI](https://platform.openai.com/signup/) or [Azure OpenAI Service](https://learn.microsoft.com/azure/cognitive-services/openai/overview#how-do-i-get-access-to-azure-openai).
1. Use your credentials to start experimenting with the [OpenAI .NET samples](https://aka.ms/openai-dotnet-samples).

## What’s next

In the next post, we’ll go into more detail about ChatGPT and how you can use OpenAI models in more conversational contexts. 

## We want to hear from you

Help us learn more about how you’re looking to use AI in your applications. Please take a few minutes to complete a short survey.

[cta-button align='center' text='Take the survey' url='https://aka.ms/dotnet-oai-survey-3'  color='#5c33b8']

Take a few minutes to complete this [survey](https://aka.ms/dotnet-oai-survey-3).

Are there any topics you’re interested in learning more about? Let us know in the comments.

## Additional resources

If you'd like to learn more techniques for building your own prompts, check out the [prompt engineering techniques](https://learn.microsoft.com/azure/cognitive-services/openai/concepts/advanced-prompt-engineering?pivots=programming-language-chat-completions) and [The Art of the Prompt: How To Get The Best Out Of Generative AI](https://news.microsoft.com/source/features/ai/the-art-of-the-prompt-how-to-get-the-best-out-of-generative-ai/) articles. 
